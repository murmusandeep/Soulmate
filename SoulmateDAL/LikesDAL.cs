using Microsoft.EntityFrameworkCore;
using SoulmateDAL.Data;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateDAL
{
    public class LikesDAL : ILikesDAL
    {
        private readonly DataContext _dataContext;

        public LikesDAL(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void AddLike(UserLike userLike)
        {
            _dataContext.Likes.Add(userLike);
        }

        public void DeleteLike(UserLike userLike)
        {
            _dataContext.Likes.Remove(userLike);
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
        {
            return await _dataContext.Likes
                .Where(x => x.SourceUserId == currentUserId)
                .Select(x => x.TargetUserId)
                .ToListAsync();
        }

        public IQueryable<UserLike> GetLikes()
        {
            return _dataContext.Likes.AsQueryable();
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _dataContext.Likes.FindAsync(sourceUserId, targetUserId);
        }
    }
}
