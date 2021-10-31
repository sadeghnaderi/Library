using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is requierd")]
        public string username { get; set; }

        [Required(ErrorMessage = "password is requierd")]
        public string password { get; set; }
    }
}
