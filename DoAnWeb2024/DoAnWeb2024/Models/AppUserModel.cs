using Microsoft.AspNetCore.Identity;

namespace DoAnWeb2024.Models
{
    public class AppUserModel : IdentityUser
    {
        public string  Occupation { get; set; }
    }
}
