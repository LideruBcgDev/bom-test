using Microsoft.AspNetCore.Mvc;
using BomManagement.PRM;
using BomManagement.MDL;
using Newtonsoft.Json;

namespace BomManagement.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JuchuBuhinApiController : BaseApiController<JuchuBuhinSearchParam, JuchuBuhinSearchResult>
    {
        private readonly JuchuBuhinSearch _juchuBuhinSearch;

        public JuchuBuhinApiController()
        {
            _juchuBuhinSearch = new JuchuBuhinSearch();
        }

        protected override object ExecuteSearch(JuchuBuhinSearchParam param)
        {
            return _juchuBuhinSearch.Search(param);
        }
    }
} 