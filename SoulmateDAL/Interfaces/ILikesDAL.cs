using SoulmateDAL.Entities;

namespace SoulmateDAL.Interfaces
{
    public interface ILikesDAL
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
        IQueryable<UserLike> GetLikes();
        void DeleteLike(UserLike userLike);
        void AddLike(UserLike userLike);
    }
}
