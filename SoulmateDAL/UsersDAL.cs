using Microsoft.EntityFrameworkCore;
using SoulmateDAL.Data;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateDAL
{
    public class UsersDAL : IUsersDAL
    {
        private readonly DataContext _dataContext;

        public UsersDAL(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<AppUser> GetUserAsync(string username)
        {
            return _dataContext.Users.AsNoTracking().Where(x => x.UserName == username).AsQueryable();
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _dataContext.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == username);
        }

        public IQueryable<AppUser> GetUsers()
        {
            return _dataContext.Users.AsNoTracking();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
