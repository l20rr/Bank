using Bank.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Bank.Shared;
using System.Linq;

namespace Bank.Api.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletModel _walletModel;

        public WalletController(IWalletModel walletModel)
        {
            _walletModel = walletModel;
        }

        [HttpGet]
        public IActionResult GetAllWallets()
        {
            try
            {
                return Ok(_walletModel.GetAllWallets());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching users: {ex.Message}");
            }
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetWalletsById(int walletId)
        {
            try
            {
                var wallet = await _walletModel.GetWalletsById(walletId);
                if (wallet != null)
                    return Ok(wallet);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the user: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWallet([FromBody] Wallet wallet)
        {
            try
            {
                var newWallet = await _walletModel.AddWallet(wallet);
                return CreatedAtAction(nameof(GetWalletsById), new { walletId = newWallet.WalletId }, newWallet);
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

        [HttpPut("{walletId}")]
        public async Task<IActionResult> UpdateWallet(int walletId, [FromBody] Wallet wallet)
        {
            try
            {
                if (walletId != wallet.WalletId)
                    return BadRequest("Wallet ID mismatch.");

                var updatedUser = await _walletModel.UpdateWallet(wallet);
                if (updatedUser != null)
                    return Ok(updatedUser);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the user: {ex.Message}");
            }
        }

        [HttpDelete("{walletId}")]
        public async Task<IActionResult> DeleteWallet(int walletId)
        {
            try
            {
                var deletedWallet= await _walletModel.DeleteWallet(walletId);
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
