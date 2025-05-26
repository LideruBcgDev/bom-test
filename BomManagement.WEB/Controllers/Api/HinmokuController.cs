using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BomManagement.BOM_MDL;
using System.Collections.Generic;
using System;

namespace BomManagement.WEB.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HinmokuController : ControllerBase
    {
        private readonly ILogger<HinmokuController> _logger;

        public HinmokuController(ILogger<HinmokuController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(string hinmokuCode)
        {
            try
            {
                // TODO: 実際のデータベース検索処理を実装
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
                    }
                };

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "部品検索でエラーが発生しました");
                return StatusCode(500, "検索処理でエラーが発生しました。");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] HinmokuInfo model)
        {
            try
            {
                if (id != model.HinmokuCode)
                {
                    return BadRequest("部品コードが一致しません。");
                }

                // TODO: 実際のデータベース更新処理を実装
                _logger.LogInformation($"部品情報を更新しました: {model.HinmokuCode}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "部品情報の更新でエラーが発生しました");
                return StatusCode(500, "更新処理でエラーが発生しました。");
            }
        }
    }
} 