using Bank.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank.Api.Models
{
    public interface IWalletModel
    {
        IEnumerable<Wallet> GetAllWallets();
        Task<Wallet> GetWalletsById(int walletId);
        Task<Wallet> AddWallet(Wallet wallet);
        Task<Wallet> UpdateWallet(Wallet wallet);
        Task<Wallet> DeleteWallet(int walletId);
    }
}
