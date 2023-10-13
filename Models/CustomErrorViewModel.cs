using Newtonsoft.Json;

namespace iBudget.Models;

public class CustomErrorViewModel
{
    public int Code { get; set; }
    public string ErrorMessage { get; set; } = String.Empty;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
