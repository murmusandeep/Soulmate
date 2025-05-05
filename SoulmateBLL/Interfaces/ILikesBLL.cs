using Shared.DataTransferObject;
using Shared.Helper;

namespace SoulmateBLL.Interfaces
{
    public interface ILikesBLL
    {
        Task<bool> ToggleLike(int sourceUserId, int targetUserId);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
        Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
    }
}
