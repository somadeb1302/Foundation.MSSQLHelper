namespace Foundation.MSSQLHelper.Models;
public class MSSQLConnectionModel
{
    public string? ServerName { get; set; }
    public string? DataBaseName { get; set; }
    public string? UserId { get; set; }
    public string? Password { get; set; }
    public int ConnectionTimeout { get; set; } = 30;
    public int ConnectionRetryCount { get; set; } = 1;
    public int ConnectionRetryInterval { get; set; } = 3;
}
