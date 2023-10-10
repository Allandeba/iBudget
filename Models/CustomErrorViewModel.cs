using Newtonsoft.Json;

namespace getQuote.Models;

public class CustomErrorViewModel
{
    public int Code { get; set; }
    public string ErrorMessage { get; set; } = String.Empty;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
