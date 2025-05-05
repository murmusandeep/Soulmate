using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObject;
using Shared.Exceptions;
using Shared.Helper;
using SoulmateBLL.Interfaces;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateBLL
{
    public class LikesBLL : ILikesBLL
    {
        private readonly ILikesDAL _likesDAL;
        private readonly IUsersDAL _usersDAL;
        private readonly IMapper _mapper;

        public LikesBLL(ILikesDAL likesDAL, IUsersDAL usersDAL, IMapper mapper)
        {
            _likesDAL = likesDAL;
            _usersDAL = usersDAL;
            _mapper = mapper;
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
        {
            return await _likesDAL.GetCurrentUserLikeIds(currentUserId);
        }

        public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
        {
            IQueryable<MemberDto> query;

            switch (likesParams.Predicate)
            {
                case "liked":
                    query = _likesDAL.GetLikes()
                        .Where(x => x.SourceUserId == likesParams.UserId)
                        .Select(x => x.TargetUser)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                    break;
                case "likedBy":
                    query = _likesDAL.GetLikes()
                        .Where(x => x.TargetUserId == likesParams.UserId)
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                    break;
                default:
                    var likeIds = await _likesDAL.GetCurrentUserLikeIds(likesParams.UserId);
                    query = _likesDAL.GetLikes()
                        .Where(x => x.TargetUserId == likesParams.UserId && likeIds.Contains(x.SourceUserId))
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                    break;

            }

            return await PagedList<MemberDto>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<bool> ToggleLike(int sourceUserId, int targetUserId)
        {
            if (sourceUserId == targetUserId) throw new BadRequestException("You cannot Liked Yourself");

            var existingLike = await _likesDAL.GetUserLike(sourceUserId, targetUserId);

            if (existingLike == null)
            {
                var like = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };

                _likesDAL.AddLike(like);
            }
            else
            {
                _likesDAL.DeleteLike(existingLike);
            }

            return await _usersDAL.SaveAllAsync();
        }
    }
}
