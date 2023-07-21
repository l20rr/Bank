using Bank.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Api.Models
{
    public class WalletModel : IWalletModel
    {
        private readonly AppDbContext _appDbContext;

        public WalletModel(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public  IEnumerable<Wallet> GetAllWallets()
        {
            return _appDbContext.Wallets;
        }

        public async Task<Wallet> GetWalletsById(int walletId)
        {
            return _appDbContext.Wallets.FirstOrDefault(c => c.WalletId == walletId);
        }

        public async Task<Wallet> AddWallet(Wallet wallet)
        {
            var result = await _appDbContext.Wallets.AddAsync(wallet);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Wallet> UpdateWallet(Wallet wallet)
        {
            var foundUser = _appDbContext.Wallets.FirstOrDefault(e => e.WalletId == wallet.WalletId);

            if (foundUser != null)
            {

                foundUser.WalletName = wallet.WalletName;

                _appDbContext.SaveChangesAsync();

                return foundUser;
            }

            return null;
        }

        public async Task<Wallet> DeleteWallet(int walletId)
        {
            var wallet = _appDbContext.Wallets.FirstOrDefault(c => c.WalletId == walletId);
            if (wallet != null)
            {
                _appDbContext.Wallets.Remove(wallet);
                await _appDbContext.SaveChangesAsync();
            }
            return null;
        }

    }
}
