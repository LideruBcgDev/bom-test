using Microsoft.AspNetCore.Mvc;
using BomManagement.PRM;
using BomManagement.MDL;
using Newtonsoft.Json;

namespace BomManagement.WEB.Controllers
{
    public class HinmokuApiController : BaseApiController<HinmokuSearchParam, HinmokuSearchResult>
    {
        private readonly HinmokuSearch _hinmokuSearch;
        private readonly HinmokuEdit _hinmokuEdit;

        public HinmokuApiController()
        {
            _hinmokuSearch = new HinmokuSearch();
            _hinmokuEdit = new HinmokuEdit();
        }

        protected override object ExecuteSearch(HinmokuSearchParam param)
        {
            return _hinmokuSearch.Search(param);
        }

        [HttpPost("edit")]
        public ActionResult<string> Edit([FromBody] HinmokuEditParam param)
        {
            try
            {
                _hinmokuEdit.Save(param);
                return Content(JsonConvert.SerializeObject(new { success = true }), "application/json");
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(new { success = false, message = ex.Message }), "application/json");
            }
        }
    }
} 