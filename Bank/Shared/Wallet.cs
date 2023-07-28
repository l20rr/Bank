using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared
{
    public class Wallet
    {
        public int WalletId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string WalletName { get; set; }
     
        public int? SymbolId { get; set; }

        public User? User { get; set; }
        public ICollection<SymbolAc>? SymbolAcs { get; set; }
    }
}
