using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(string userName, string password);

        Task<bool> CheckPassword(User user, string password);
    }

    public class UserService : IUserService
    {

        public async Task<User> CreateUser(string userName, string password)
        {
            var salt = GenerateSalt();
            return new User { Password = GeneratePasswordHash(password, salt), UserName = userName, Salt = salt };
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            if (user == null)
                throw new ArgumentException("User is null");

            if (GeneratePasswordHash(password, user.Salt) == user.Password)
                return true;
            return false;
        }

        private string GeneratePasswordHash(string password, string salt)
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

        private string GenerateSalt()
        {
            var random = new Random(DateTime.Now.Millisecond);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
