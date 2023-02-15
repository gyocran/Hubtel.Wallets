using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallets.Api.DTOs
{
    public class WalletDto
    {
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string AccountScheme { get; set; }
        [Required]
        public string Owner { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
