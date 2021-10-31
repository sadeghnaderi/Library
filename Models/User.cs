using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("UserType")]
        public int UserTypeId { get; set; }
        public virtual UserType UserType { get; set; }

        [Required]
        public string Name { get; set; }

        public string Username { get; set; }

    }

}
