using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Hubtel.Wallets.Api.Models
{
    public class Wallet : IEquatable<Wallet>
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(16)]
        public string AccountNumber { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(10)]
        public string Owner { get; set; }
        public AccountScheme AccountScheme { get; set; }
        public AccountType AccountType { get; set; }

        public bool Equals([AllowNull] Wallet other)
        {
            return (
                other.ID == ID &&
                other.Name == Name &&
                other.AccountNumber == AccountNumber &&
                other.CreatedAt == CreatedAt &&
                other.Owner == Owner &&
                other.AccountScheme.ID == AccountScheme.ID &&
                other.AccountScheme.Scheme == AccountScheme.Scheme &&
                other.AccountType.ID == AccountType.ID &&
                other.AccountType.Type == AccountType.Type
                );
        }
    }
}
