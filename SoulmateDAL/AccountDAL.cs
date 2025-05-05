using Microsoft.EntityFrameworkCore;
using SoulmateDAL.Data;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateDAL
{
    public class AccountDAL : IAccountDAL
    {
        private readonly DataContext _context;
        public AccountDAL(DataContext context)
        {
            _context = context;
        }

        public async Task Register(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }

        public async Task<AppUser> GetUser(string username)
        {
            return await _context.Users.Include(P => P.Photos).SingleOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
        }
    }
}
