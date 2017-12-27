using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyTwitter.Models
{
    [Table("Posts")]
    public class Post
    {
        public int Id { get; set; }

        [ForeignKey("IdApplicationUser")]
        public ApplicationUser ApplicationUser { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Post()
        {
            DateCreated = DateUpdated = DateTime.Now;
        }
    }
}
