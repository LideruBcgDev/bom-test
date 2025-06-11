using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BomManagement.BOM_MDL;
using System.Collections.Generic;
using System;
using System.Data;

namespace BomManagement.WEB.Controllers
{
    [Authorize]
    public class HinmokuController : Controller
    {
        private readonly ILogger<HinmokuController> _logger;

        public HinmokuController(ILogger<HinmokuController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var command = CommandFactory.GetInstance().CreateCommand("Hinmoku/Index");
                var result = command.Execute();
                
                if (result is DataTable dt)
                {
                    ViewBag.SearchResult = dt;
                }
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "部品一覧の表示でエラーが発生しました");
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult Search(HinmokuSearchParam param)
        {
            try
            {
                var command = CommandFactory.GetInstance().CreateCommand("Hinmoku/Search");
                command.Parameters = param;
                var result = command.Execute();

                if (result is DataTable dt)
                {
                    ViewBag.SearchResult = dt;
                    ViewBag.SearchParam = param;
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "部品検索でエラーが発生しました");
                return Json(new { success = false, message = "検索処理でエラーが発生しました。" });
            }
        }

        public IActionResult Edit(string id)
        {
            try
            {
                var command = CommandFactory.GetInstance().CreateCommand("Hinmoku/Edit");
                command.Parameters = new { Id = id };
                var result = command.Execute();

                if (result is HinmokuInfo hinmoku)
                {
                    return View(hinmoku);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "部品情報の取得でエラーが発生しました");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HinmokuInfo model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var command = CommandFactory.GetInstance().CreateCommand("Hinmoku/Update");
                command.Parameters = model;
                command.Execute();

                _logger.LogInformation($"部品情報を更新しました: {model.HinmokuCode}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "部品情報の更新でエラーが発生しました");
                ModelState.AddModelError("", "更新処理でエラーが発生しました。");
                return View(model);
            }
        }
    }
} 