using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hubtel.Wallets.Api.Models
{
    public class AccountType
    {

        [Key]
        public int ID { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
