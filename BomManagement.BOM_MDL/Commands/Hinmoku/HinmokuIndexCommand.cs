using System.Data;

namespace BomManagement.BOM_MDL.Commands.Hinmoku
{
    public class HinmokuIndexCommand : CommandBase
    {
        public override object Execute()
        {
            // TODO: 実際のデータベースから部品一覧を取得
            DataTable dt = new DataTable();
            dt.Columns.Add("HinmokuCode", typeof(string));
            dt.Columns.Add("HinmokuName", typeof(string));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("CreatedDate", typeof(DateTime));
            dt.Columns.Add("UpdatedDate", typeof(DateTime));

            // 仮のデータを追加
            dt.Rows.Add("A001", "テスト部品1", "個", 1000, DateTime.Now, DateTime.Now);
            dt.Rows.Add("A002", "テスト部品2", "個", 2000, DateTime.Now, DateTime.Now);

            return dt;
        }
    }
} 