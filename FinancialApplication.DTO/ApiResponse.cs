using System.Text.Json;

namespace FinancialApplication.DTO;

public class ApiResponse<T>
{
    public ApiResponse() { }
    public ApiResponse(T data)
    {
        data = data;
    }
    public bool hasError { get; set; }
    public string message { get; set; }
    public int statusCode { get; set; }
    public T data { get; set; }
    public override string ToString() => JsonSerializer.Serialize(this);
}
