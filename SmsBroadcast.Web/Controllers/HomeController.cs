using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmsBroadcast.Core;
using SmsBroadcast.Core.Entities;
using SmsBroadcast.Core.Services;
using SmsBroadcast.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using SmsBroadcast.Core.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace SmsBroadcast.Web.Controllers
{
    [Authorize(Roles ="ACL-SmsBroadcast, ACL-Developers")]
    public class HomeController : Controller
    {
        private readonly ISmsBroadcast _smsBroadcastService;

        public HomeController(ISmsBroadcast smsBroadcast)
        {
            _smsBroadcastService = smsBroadcast;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BroadcastScheduleViewModel model, IFormFile csvUploadFile)
        {
            if (csvUploadFile == null)
            {
                ModelState.AddModelError(string.Empty, "CSV file with numbers to send broadcast is required.");
                return View(model);
            }
            else
            {
                var csvFileUploadPath = Path.GetTempFileName();
                if (csvUploadFile.Length > 0)
                {
                    if (ModelState.IsValid)
                    {
                        using (var stream = new FileStream(csvFileUploadPath, FileMode.Create))
                        {
                            await csvUploadFile.CopyToAsync(stream);
                        }

                        List<string> recipients = await GetRecipients(csvFileUploadPath);

                        if (model.RunOnce)
                        {
                            await _smsBroadcastService.ProcessBroadcastAsync(
                                new Broadcast
                                {
                                    Code = model.Code,
                                    CreatedBy = User.Identity.Name.Substring(4),
                                    Description = model.Description,
                                    From = model.From,
                                    Message = model.Message,
                                    RepeatEndDate = null,
                                    ScheduleDateTime = DateTimeOffset.UtcNow,
                                    Status = "PENDING",
                                    Subject = model.Subject
                                },
                                recipients.ToArray());
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        TempData["Message"] = "Broadcast messages submitted for queueing";

                        return RedirectToAction("index");
                    }

                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "CSV file with numbers to send broadcast is required.");
                return View(model);
            }
        }

        private static async Task<List<string>> GetRecipients(string csvFileUploadPath)
        {
            var csvDataBuilder = new StringBuilder();
            csvDataBuilder.Clear();

            using (StreamReader reader = new StreamReader(csvFileUploadPath, encoding: Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    csvDataBuilder.Append($"{await reader.ReadLineAsync()},");
                }
            }

            var recipients = new List<string>();
            var csvData = (csvDataBuilder.ToString()).Split(',').Skip(1);
            var msisdnValidator = new Regex("^71[0-9]{7}$");

            foreach (var record in csvData)
            {
                if (!string.IsNullOrWhiteSpace(record) && msisdnValidator.IsMatch(record))
                {
                    recipients.Add(record);
                }
            }

            return recipients;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
