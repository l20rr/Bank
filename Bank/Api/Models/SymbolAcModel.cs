using Bank.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Api.Models
{
    public class SymbolAcModel : ISymbolAcModel
    {
        private readonly AppDbContext _appDbContext;

        public SymbolAcModel(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<SymbolAc> AddSymbol(SymbolAc symbolAc)
        {
            var result = await _appDbContext.SymbolAcs.AddAsync(symbolAc);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<SymbolAc> DeleteSymbol(int symbolId)
        {
            var symbol = _appDbContext.SymbolAcs.FirstOrDefault(c => c.SymbolId == symbolId);
            if (symbol != null)
            {
                _appDbContext.SymbolAcs.Remove(symbol);
                await _appDbContext.SaveChangesAsync();
            }
            return null;
        }

        public  IEnumerable<SymbolAc> GetAllSymbols()
        {
            return _appDbContext.SymbolAcs;
        }

        public async Task<SymbolAc> GetSymbolById(int symbolId)
        {
            return _appDbContext.SymbolAcs.FirstOrDefault(c => c.SymbolId == symbolId);
        }
    }
}
