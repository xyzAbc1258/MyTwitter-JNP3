using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyTwitter.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {
        [InverseProperty("ApplicationUser")]
        public List<Post> Posts { get; set; }

        public ApplicationUser()
        {
            Posts = new List<Post>();
        }
    }
}
