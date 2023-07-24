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
        public  IActionResult GetWalletsById(int walletId)
        {
            try
            {
                return Ok(_walletModel.GetWalletsById(walletId));
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
        public  IActionResult UpdateWallet(int walletId, [FromBody] Wallet wallet)
        {
            if (wallet == null)
                return BadRequest();

         

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToUpdate = _walletModel.GetWalletsById(wallet.WalletId);

            if (userToUpdate == null)
                return NotFound();

            _walletModel.UpdateWallet(wallet);

            return NoContent(); //success
        }

        [HttpDelete("{walletId}")]
        public IActionResult DeleteWallet(int walletId)
        {
            if (walletId == 0)
                return BadRequest();

            var WalletToDelete = _walletModel.GetWalletsById(walletId);
            if (WalletToDelete == null)
                return NotFound();

            _walletModel.DeleteWallet(walletId);

            return NoContent();//success
        }
    }
}
