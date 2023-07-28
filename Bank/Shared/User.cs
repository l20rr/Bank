using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime JoinedDate { get; set; }
        [Required]
        public string UPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

        public Wallet? Wallet { get; set; }
    }
}
