using Microsoft.AspNetCore.Mvc;
using BomManagement.PRM;
using BomManagement.MDL;
using Newtonsoft.Json;

namespace BomManagement.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SekkeiBuhinApiController : ControllerBase
    {
        private readonly SekkeiBuhinSearch _sekkeiBuhinSearch;

        public SekkeiBuhinApiController()
        {
            _sekkeiBuhinSearch = new SekkeiBuhinSearch();
        }

        [HttpPost("search")]
        public ActionResult<string> Search([FromBody] SekkeiBuhinSearchParam param)
        {
            var result = _sekkeiBuhinSearch.Search(param);
            var json = JsonConvert.SerializeObject(result);
            return Content(json, "application/json");
        }
    }
} 