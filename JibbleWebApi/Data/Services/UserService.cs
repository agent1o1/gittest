using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IUserService
    {
        string GenerateSalt();
        string GeneratePasswordHash(string password, string salt);
    }

    public class UserService : IUserService
    {
        public string GeneratePasswordHash(string password, string salt)
        {
            var hashAlgorithm = SHA256.Create();
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes($"{password}.{salt}"));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public string GenerateSalt()
{
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[12];

            rng.GetBytes(buffer);
            return BitConverter.ToString(buffer);
        }
    }
}
