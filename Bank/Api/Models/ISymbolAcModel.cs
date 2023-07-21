using Bank.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank.Api.Models
{
    public interface ISymbolAcModel
    {
        IEnumerable<SymbolAc> GetAllSymbols();
        Task<SymbolAc> GetSymbolById(int symbolId);
        Task<SymbolAc> AddSymbol(SymbolAc symbolAc);
        Task<SymbolAc> DeleteSymbol(int symbolId);
    }
}
