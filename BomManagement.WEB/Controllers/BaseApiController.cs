using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace BomManagement.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController<TParam, TResult> : ControllerBase
        where TParam : class
        where TResult : class
    {
        protected abstract object ExecuteSearch(TParam param);

        [HttpPost("search")]
        public ActionResult<string> Search([FromBody] TParam param)
        {
            var result = ExecuteSearch(param);
            var json = JsonConvert.SerializeObject(result);
            return Content(json, "application/json");
        }
    }
} 