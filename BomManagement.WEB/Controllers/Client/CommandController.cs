using BomManagement.FW_WEB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BomManagement.WEB.Controllers.Client
{
    [Authorize]
    [ApiController]
    [Route("client")]
    public class CommandController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;

        public CommandController(ILogger<CommandController> logger)
        {
            _logger = logger;
        }

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

    //public class CommandRequest
    //{
    //    public string CommandName { get; set; }
    //    public Dictionary<string, object> Parameters { get; set; }
    //}
} 