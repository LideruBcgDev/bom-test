using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BomManagement.BOM_MDL;
using System.Collections.Generic;
using System;

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
            return View();
        }

        [HttpPost]
        public IActionResult Search(string hinmokuCode)
        {
            try
            {
                // TODO: 実際のデータベース検索処理を実装
                // 仮のデータを返す
                var results = new List<HinmokuInfo>
                {
                    new HinmokuInfo
                    {
                        HinmokuCode = "A001",
                        HinmokuName = "テスト部品1",
                        Unit = "個",
                        Price = 1000,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    },
                    new HinmokuInfo
                    {
                        HinmokuCode = "A002",
                        HinmokuName = "テスト部品2",
                        Unit = "個",
                        Price = 2000,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    }
                };

                return Json(new { success = true, data = results });
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
                // TODO: 実際のデータベースから部品情報を取得
                // 仮のデータを返す
                var hinmoku = new HinmokuInfo
                {
                    HinmokuCode = id,
                    HinmokuName = "テスト部品" + id,
                    Unit = "個",
                    Price = 1000,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                return View(hinmoku);
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

                // TODO: 実際のデータベース更新処理を実装
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