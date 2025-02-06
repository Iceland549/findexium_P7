using Microsoft.AspNetCore.Identity;

namespace P7CreateRestApi.Domain
{
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        //public string? UserName { get; set; }
        public string? Password {  get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}