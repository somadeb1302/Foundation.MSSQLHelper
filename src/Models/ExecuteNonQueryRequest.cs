using System.Data;

namespace Foundation.MSSQLHelper.Models;
public class ExecuteNonQueryRequest
{

    public MSSQLConnectionModel? ConnectionProperties { get; set; }
    public string? CommandText { get; set; }
    public CommandType CommandType { get; set; }
    public int CommandTimeout { get; set; }
    public IDbDataParameter []? Parameters { get; set; }
}
