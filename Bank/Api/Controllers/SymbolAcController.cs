using Bank.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Bank.Shared;

namespace Bank.Api.Controllers
{
    [Route("api/symbols")]
    [ApiController]
    public class SymbolAcController : ControllerBase
    {
        private readonly ISymbolAcModel _symbolAcModel;

        public SymbolAcController(ISymbolAcModel symbolAcModel)
        {
            _symbolAcModel = symbolAcModel;
        }


        [HttpGet]
        public IActionResult GetAllSymbols()
        {
            try
            {
                return Ok(_symbolAcModel.GetAllSymbols());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching users: {ex.Message}");
            }
        }



        [HttpGet("{symbolId}")]
        public async Task<IActionResult> GetSymbolById(int symbolId)
        {
            try
            {
                var symbol = await _symbolAcModel.GetSymbolById(symbolId);
                if (symbol != null)
                    return Ok(symbol);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the user: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddSymbol(SymbolAc symbolAc)
        {
            try
            {
                var newSymbolAc = await _symbolAcModel.AddSymbol(symbolAc);
                return CreatedAtAction(nameof(GetSymbolById), new { symbolId = newSymbolAc.SymbolId }, newSymbolAc);
            }
            catch (Exception ex)
            {
                // Retornar uma resposta BadRequest com os detalhes do erro interno.
                return BadRequest(new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = $"An error occurred while adding the wallet: {ex.Message}",
                    Status = 500
                });
            }
        }




        [HttpDelete("{symbolId}")]
        public async Task<IActionResult> DeleteSymbol(int symbolId)
        {
            try
            {
                var deletedWallet = await _symbolAcModel.DeleteSymbol(symbolId);
                if (deletedWallet != null)
                    return Ok(deletedWallet);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }









    }
}
