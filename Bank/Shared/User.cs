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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime JoinedDate { get; set; }
        public string UPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public Wallet? Wallet { get; set; }
    }
}
