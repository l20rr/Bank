using Bank.Shared;

namespace Bank.Client.Services
{
    public interface ISymbolAcService
    {
        Task<IEnumerable<SymbolAc>> GetAllSymbols();
        Task<SymbolAc> AddSymbol(SymbolAc symbolAc);
        Task DeleteSymbol(int symbolId);

        Task<IEnumerable<SymbolAc>> ISymbolAcService();
    }
}
