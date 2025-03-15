using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace iBudget.Framework;

public class Cryptography
{
    public string GetHash(string text)
    {
        if (text.IsNullOrEmpty())
            throw new Exception("Cryptography.GetHash() must not be null or empty");

        using (var sha256 = SHA256.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(text);
            var hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}