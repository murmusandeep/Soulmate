namespace SoulmateDAL.Entities
{
    public class AppUserWithRoles
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
