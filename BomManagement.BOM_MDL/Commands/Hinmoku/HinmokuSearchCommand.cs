using System.Data;

namespace BomManagement.BOM_MDL.Commands.Hinmoku
{
    public class HinmokuSearchCommand : CommandBase
    {
        public override object Execute()
        {
            var param = Parameters as HinmokuSearchParam;
            if (param == null)
            {
                throw new ArgumentException("パラメータが不正です。");
            }

            // TODO: 実際のデータベースから部品を検索
            DataTable dt = new DataTable();
            dt.Columns.Add("HinmokuCode", typeof(string));
            dt.Columns.Add("HinmokuName", typeof(string));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("CreatedDate", typeof(DateTime));
            dt.Columns.Add("UpdatedDate", typeof(DateTime));

            // 検索条件に基づいてデータを追加
            if (string.IsNullOrEmpty(param.HinmokuCode) || param.HinmokuCode == "A001")
            {
                dt.Rows.Add("A001", "テスト部品1", "個", 1000, DateTime.Now, DateTime.Now);
            }
            if (string.IsNullOrEmpty(param.HinmokuCode) || param.HinmokuCode == "A002")
            {
                dt.Rows.Add("A002", "テスト部品2", "個", 2000, DateTime.Now, DateTime.Now);
            }

            return dt;
        }
    }
} 