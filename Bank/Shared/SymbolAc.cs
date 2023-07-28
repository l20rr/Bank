using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared
{
    public class SymbolAc
    {
        public int SymbolId { get; set; }
        [Required]
        public int WalletId { get; set; }
        [Required]
        public string SymbolName { get; set; }

        public Wallet? Wallet { get; set; }
    }
}
