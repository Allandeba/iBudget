using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace iBudget.Framework;

public class Cryptography
{
    public string GetHash(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new Exception("Cryptography.GetHash() must not be null or empty");

        var sha256 = SHA256.Create();
        var base64 = Encoding.Default.GetBytes(text);
        var hashText = sha256.ComputeHash(base64);

        return Convert.ToBase64String(hashText);
    }
}