namespace SentinelCore.DAL.Data.Models
{
    public class AppRoles
    {
        public static readonly Role[] Roles = new[] { Role.Admin, Role.User };
        public enum Role
        {
            Admin,
            User,
        }
    }
}