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

                var error = _service.AddWallet(wallet);

                if (!string.IsNullOrEmpty(error))
                    return BadRequest(error);

                //wallet.AccountNumber

                //_repo.AddWallet(D);
                return Ok("wallet created");
                //return Created();    
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
