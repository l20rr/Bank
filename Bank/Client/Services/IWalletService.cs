using Bank.Shared;

namespace Bank.Client.Services
{
    public interface IWalletService
    {
        Task<IEnumerable<Wallet>> GetAllWallets();
        Task<Wallet> AddWallet(Wallet wallet);
        Task UpdateWallet(Wallet wallet);
        Task DeleteWallet(int walletId);
        Task<Wallet> GetWalletId(int walletId);
        Task<IEnumerable<Wallet>> IWalletService();
     
    }
}
