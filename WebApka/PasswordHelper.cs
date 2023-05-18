
using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{
    public static string EncryptPassword(string password)
    {
        using (var md5 = MD5.Create())
        {
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}