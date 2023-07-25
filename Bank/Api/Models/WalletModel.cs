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

   
        public async Task<Wallet> AddWallet(Wallet wallet)
        {
            var result = await _appDbContext.Wallets.AddAsync(wallet);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public void DeleteWallet(int walletId)
        {
            var foundWallets = _appDbContext.Wallets.FirstOrDefault(e => e.WalletId == walletId);
            if (foundWallets == null) return;

            _appDbContext.Wallets.Remove(foundWallets);
            _appDbContext.SaveChanges();
        }

        public Wallet GetWalletsById(int walletId)
        {
            return _appDbContext.Wallets.FirstOrDefault(c => c.WalletId == walletId);
        }

        public Wallet UpdateWallet(Wallet wallet)
        {
            var foundWallet= _appDbContext.Wallets.FirstOrDefault(e => e.WalletId == wallet.WalletId);

            if (foundWallet != null)
            {

                foundWallet.UserId = wallet.UserId;
                foundWallet.WalletName = wallet.WalletName;
             


                _appDbContext.SaveChangesAsync();

                return foundWallet;
            }

            return null;
        }
    }
}
