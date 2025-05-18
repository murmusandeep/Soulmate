using Microsoft.AspNetCore.Identity;

namespace SoulmateDAL.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
