using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared
{
    public class SymbolAc
    {
        public int SymbolId { get; set; }
        public int WalletId { get; set; }
        public string SymbolName { get; set; }

        public Wallet? Wallet { get; set; }
    }
}
