using System;
using System.Threading.Tasks;
using DATINGAPP.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DATINGAPP.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user= await _context.Users.FirstOrDefaultAsync(u=>u.Name==username);
            if(user==null) return null;
            if (!VerifyPassword(password,user.PasswordHash,user.PasswordSalt)) return null;
            return user;
            
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
           using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               for (int i = 0; i < ComputeHash.Length; i++)
               {
                   if (ComputeHash[i]!=passwordHash[i]) return false;
               }
               return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] PasswordHash,PasswordSalt;
            CreatePassword(password,out PasswordHash,out PasswordSalt);
            user.PasswordHash=PasswordHash;
            user.PasswordSalt=PasswordSalt;
             await _context.Users.AddAsync(user);
             await _context.SaveChangesAsync();
             return user;
        }

        private void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                passwordSalt=hmac.Key;
            }
            
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x=>x.Name==username)) return true;
            return false;

        }
    }
}