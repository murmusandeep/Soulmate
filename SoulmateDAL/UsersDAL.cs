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

        public async Task<AppUser> GetUser(string username)
        {
            return await _dataContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.UserName == username.ToLower());
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _dataContext.Users.ToListAsync();
        }
    }
}
