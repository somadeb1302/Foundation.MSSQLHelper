using Foundation.MSSQLHelper.Models;

namespace Foundation.MSSQLHelper.MSSQLHelper;
public interface ISqlHelper
{
    Task<dynamic?> FetchData(ExecuteDataSetRequest request);
    Task<int> ExecuteNonQuery(ExecuteNonQueryRequest request);
}
