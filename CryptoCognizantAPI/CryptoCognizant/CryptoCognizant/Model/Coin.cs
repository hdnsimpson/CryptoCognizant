using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCognizant.Model
{
    public partial class Coin
    {
        public int CoinId { get; set; }
        [Required]
        [StringLength(255)]
        public string CoinSymbol { get; set; }
        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }
        [Column("isFavourite")]
        public bool IsFavourite { get; set; }
    }
}
