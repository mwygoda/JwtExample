using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JWTExample.DataAccess.Models
{
    public class User : IdentityUser
    {
        [Required, MaxLength(256)]
        public string FirstName { get; set; }
        [Required, MaxLength(256)]
        public string LastName { get; set; }
    }
}