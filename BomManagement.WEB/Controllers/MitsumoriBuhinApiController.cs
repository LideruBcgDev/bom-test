using Microsoft.AspNetCore.Mvc;
using BomManagement.PRM;
using BomManagement.MDL;
using Newtonsoft.Json;

namespace BomManagement.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MitsumoriBuhinApiController : BaseApiController<MitsumoriBuhinSearchParam, MitsumoriBuhinSearchResult>
    {
        private readonly MitsumoriBuhinSearch _mitsumoriBuhinSearch;

        public MitsumoriBuhinApiController()
        {
            _mitsumoriBuhinSearch = new MitsumoriBuhinSearch();
        }

        protected override object ExecuteSearch(MitsumoriBuhinSearchParam param)
        {
            return _mitsumoriBuhinSearch.Search(param);
        }
    }
} 