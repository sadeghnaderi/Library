using Library.IdentityAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
       
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public string ReservedDate { get; set; }

        public string ReturnedDate { get; set; }

        public string TotalLateFee { get; set; }

        [ForeignKey("ReserveState")]
        public int ReserveStateId { get; set; }
        public virtual ReserveState ReserveState { get; set; }
    }
}
