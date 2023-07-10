using System.ComponentModel;
using Domain.DomainModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class CinemaApplicationUser : IdentityUser<String>
    {
        [DisplayName("Role")]
        public Roles Role { get; set; }
        
        [DisplayName("FirstName")]
        public string FirstName { get; set; }
        
        [DisplayName("LastName")]
        public string? LastName { get; set; }
        
        [DisplayName("Owner's_Card")]
        public ShoppingCart OwnersCard { get; set; }
        public string NormalizedUserName { get; set; }
    }

}