using Microsoft.AspNetCore.Http;
using Shared.DataTransferObject;

namespace SoulmateBLL.Interfaces
{
    public interface IPhotoBLL
    {
        public Task<PhotoDto> AddPhotoAsync(string username, IFormFile file);
        public Task<bool> DeletePhotoAsync(string username, int photoId);
        public Task<bool> SetMainPhoto(string username, int photoId);
    }
}
