using System.Text.Json;

namespace FinancialApplication.DTO;

public class ApiResponse
{
    public bool hasError { get; set; }
    public string message { get; set; }
    public int statusCode { get; set; }
    public object data { get; set; }
    public override string ToString() => JsonSerializer.Serialize(this);
}
