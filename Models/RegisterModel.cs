using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="Username is requierd")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email is requierd")]
        public string email { get; set; }

        [Required(ErrorMessage = "Name is requierd")]
        public string name { get; set; }

        [Required(ErrorMessage = "Password is requierd")]
        public string password { get; set; }
    }
}
