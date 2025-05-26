using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BomManagement.MDL;
using BomManagement.PRM;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace BomManagement.WEB.Controllers
{
    [ApiController]
    [Route("client")]
    [Authorize]
    public class CommandController : ControllerBase
    {
        [HttpPost("{*commandPath}")]
        public ActionResult<string> ExecuteCommand(string commandPath, [FromBody] object param)
        {
            try
            {
                // 認証チェック
                if (!User.Identity.IsAuthenticated)
                {
                    var errorResult = new OParamBase
                    {
                        Success = false,
                        Message = "ログインが必要です。"
                    };
                    return Content(JsonConvert.SerializeObject(errorResult), "application/json");
                }

                // コマンドの作成
                var command = CommandFactory.GetInstance().CreateCommand(commandPath);

                // パラメータのデシリアライズ
                var paramType = command.GetType().GetProperty("IParam").PropertyType;
                var typedParam = (IParamBase)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(param), paramType);

                // コマンドの実行
                var executeMethod = command.GetType().GetMethod("Execute");
                var result = executeMethod.Invoke(command, new[] { typedParam });

                // 結果のシリアライズ
                var json = JsonConvert.SerializeObject(result);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                var errorResult = new OParamBase
                {
                    Success = false,
                    Message = ex.Message
                };
                return Content(JsonConvert.SerializeObject(errorResult), "application/json");
            }
        }
    }
} 