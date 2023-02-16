using Hubtel.Wallets.Api.Data;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hubtel.Wallets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        IWalletService _service;

        public WalletController(IWalletService service) 
        {
            _service = service;
        }

        // GET api/<WalletController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var wallet = _service.GetWalletById(id);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        // GET api/<WalletController>/5
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var wallets = _service.GetAllWallets();
                return Ok(wallets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<WalletController>
        [HttpPost]
        public IActionResult Post([FromBody] WalletDto wallet)
        {
            try
            {
                if (wallet == null)
                    return BadRequest(ModelState);

                if (_service.AccountNumberContainsNonNumeric(wallet))
                    return BadRequest("Account number contains non numeric characters");

                if (_service.OwnerContainsNonNumeric(wallet))
                    return BadRequest("Owner contains non numeric characters");

                if (_service.WalletAlreadyExists(wallet))
                    return BadRequest("Wallet already exists");

                if (_service.WalletCountExceeded(wallet))
                    return BadRequest("Wallet count limit exceeded");

                if (_service.SchemeDoesNotExist(wallet))
                    return BadRequest("Scheme does not exist");

                if (_service.TypeDoesNotExist(wallet))
                    return BadRequest("Type does not exist");

                if (_service.AccountNumberLengthIsInvalid(wallet))
                    return BadRequest("Account number length invalid");

                var id = _service.AddWallet(wallet);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<WalletController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return NotFound();

                var deleted = _service.DeleteWallet(id);

                if (deleted)
                    return Accepted();
                else
                    return NotFound();
            }
            catch(Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
