using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace iBudget.Framework
{
    public class Cryptography
    {
        public string GetHash(string text)
        {
            if (text.IsNullOrEmpty())
            {
                throw new Exception("Cryptography.GetHash() must not be null or empty");
            }

            SHA256 sha256 = SHA256.Create();
            byte[] base64 = Encoding.Default.GetBytes(text);
            byte[] hashText = sha256.ComputeHash(base64);

            return Convert.ToBase64String(hashText);
        }
    }
}
