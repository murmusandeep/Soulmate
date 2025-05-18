namespace Shared.DataTransferObject
{
    public class GroupDto
    {
        public required string Name { get; set; }
        public ICollection<ConnectionDto> Connections { get; set; } = [];
    }
}
