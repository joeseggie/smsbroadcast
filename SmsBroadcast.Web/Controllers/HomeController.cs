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

                        var csvData = await System.IO.File.ReadAllTextAsync(csvFileUploadPath);
                        var recipients = new List<string>();

                        foreach (var record in csvData.Split("\r\n").Skip(1))
                        {
                            if (!string.IsNullOrWhiteSpace(record))
                            {
                                recipients.Add(record);
                            }
                        }

                        if (model.RunOnce)
                        {
                            await _smsBroadcastService.ProcessBroadcastAsync(
                                new Broadcast
                                {
                                    Code = model.Code,
                                    CreatedBy = User.Identity.Name,
                                    Description = model.Description,
                                    From = model.From,
                                    Message = model.Message,
                                    RepeatEndDate = model.RepeatEndDate,
                                    ScheduleDateTime = model.ScheduleDate,
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
