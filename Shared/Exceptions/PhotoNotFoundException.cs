namespace Shared.Exceptions
{
    public sealed class PhotoNotFoundException : NotFoundException
    {
        public PhotoNotFoundException(int photoId) : base($"The Photo with photoId: {photoId} doesn't exist in the database.") { }
    }
}
