using Bank.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank.Api.Models
{
    public interface IWalletModel
    {
        IEnumerable<Wallet> GetAllWallets();
        Wallet GetWalletsById(int walletId);
        Task<Wallet> AddWallet(Wallet wallet);
        Wallet UpdateWallet(Wallet wallet);
        void  DeleteWallet(int walletId);
    }
}
