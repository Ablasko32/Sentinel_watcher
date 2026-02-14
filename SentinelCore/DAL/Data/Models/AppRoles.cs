namespace SentinelCore.DAL.Data.Models
{
    public class AppRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static readonly string[] Roles = new[] { Admin, User };
    }
}