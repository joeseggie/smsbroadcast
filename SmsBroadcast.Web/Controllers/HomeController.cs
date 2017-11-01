﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmsBroadcast.Infrastructure;
using SmsBroadcast.Infrastructure.Entities;
using SmsBroadcast.Services;
using SmsBroadcast.Web.Models;

namespace SmsBroadcast.Web.Controllers
{
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
        public async Task<IActionResult> Index(BroadcastScheduleViewModel model)
        {
            var operationResult = new OperationResult();

            if (ModelState.IsValid)
            {
                if(model.RunOnce)
                {
                    operationResult = await ScheduleRunOnceBroadcastAsync(model);
                }

                if(model.Schedule)
                {
                    operationResult = await ScheduleBroadcastAsync(model);
                }

                TempData["Message"] = operationResult.Message;

                return RedirectToAction("index");
            }

            return View(model);
        }

        private async Task<OperationResult> ScheduleBroadcastAsync(BroadcastScheduleViewModel model)
        {
            switch (model.Repeat.ToLower())
            {
                case "daily":
                    return await ScheduleDailyBroadcastAsync(model);
                case "weekly":
                    return await ScheduleWeeklyBroadcastAsync(model);
                case "monthly":
                    return await ScheduleMonthlyBroadcastAsync(model);
                case "yearly":
                    return await ScheduleYearlyBroadcastAsync(model);
                default:
                    return await ScheduleOnceBroadcastAsync(model);
            }
        }

        private async Task<OperationResult> ScheduleOnceBroadcastAsync(BroadcastScheduleViewModel model)
        {
            return await _smsBroadcastService.ScheduleBroadcastAsync(model.To.Split(new char[]{',',' '}), new Schedule{
                From = model.From,
                Subject = model.Subject,
                Message = model.Message,
                Description = model.Description,
                Code = model.Code,
                CreatedBy = User.Identity.Name.Substring(4),
                Status = "PENDING"
            }, model.ScheduleDate);
        }

        private async Task<OperationResult> ScheduleYearlyBroadcastAsync(BroadcastScheduleViewModel model)
        {
            return await _smsBroadcastService.YearlyBroadcastAsync(model.To.Split(new char[]{',',' '}), new Schedule{
                From = model.From,
                Subject = model.Subject,
                Message = model.Message,
                Description = model.Description,
                Code = model.Code,
                CreatedBy = User.Identity.Name.Substring(4),
                Status = "PENDING"
            }, model.Frequency, model.ScheduleDate);
        }

        private async Task<OperationResult> ScheduleMonthlyBroadcastAsync(BroadcastScheduleViewModel model)
        {
            return await _smsBroadcastService.MonthlyBroadcastAsync(model.To.Split(new char[]{',',' '}), new Schedule{
                From = model.From,
                Subject = model.Subject,
                Message = model.Message,
                Description = model.Description,
                Code = model.Code,
                CreatedBy = User.Identity.Name.Substring(4),
                Status = "PENDING"
            }, model.Frequency, model.ScheduleDate);
        }

        private async Task<OperationResult> ScheduleWeeklyBroadcastAsync(BroadcastScheduleViewModel model)
        {
            return await _smsBroadcastService.WeeklyBroadcastAsync(model.To.Split(new char[]{',',' '}), new Schedule{
                From = model.From,
                Subject = model.Subject,
                Message = model.Message,
                Description = model.Description,
                Code = model.Code,
                CreatedBy = User.Identity.Name.Substring(4),
                Status = "PENDING"
            }, model.Frequency, model.ScheduleDate);
        }

        private async Task<OperationResult> ScheduleDailyBroadcastAsync(BroadcastScheduleViewModel model)
        {
            return await _smsBroadcastService.DailyBroadcastAsync(model.To.Split(new char[]{',',' '}), new Schedule{
                From = model.From,
                Subject = model.Subject,
                Message = model.Message,
                Description = model.Description,
                Code = model.Code,
                CreatedBy = User.Identity.Name.Substring(4),
                Status = "PENDING"
            }, model.Frequency, model.ScheduleDate);
        }

        private async Task<OperationResult> ScheduleRunOnceBroadcastAsync(BroadcastScheduleViewModel model)
        {
            return await _smsBroadcastService.BroadcastOnceAsync(model.To.Split(new char[]{',',' '}), new Schedule{
                From = model.From,
                Subject = model.Subject,
                Message = model.Message,
                Description = model.Description,
                Code = model.Code,
                CreatedBy = User.Identity.Name.Substring(4),
                Status = "PENDING"
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
