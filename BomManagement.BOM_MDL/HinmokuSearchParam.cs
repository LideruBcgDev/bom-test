using System;

namespace BomManagement.BOM_MDL
{
    public class HinmokuSearchParam : OParamBase
    {
        public string HinmokuCode { get; set; }
        public string HinmokuName { get; set; }
        public string Unit { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
    }
} 