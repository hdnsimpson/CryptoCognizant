using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCognizant.Model
{
    public partial class Coin
    {
        public Coin()
        {
            Exchange = new HashSet<Exchange>();
        }

        public int CoinId { get; set; }
        [Required]
        [StringLength(255)]
        public string CoinSymbol { get; set; }
        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }
        [Column("isFavourite")]
        public bool IsFavourite { get; set; }

        public virtual ICollection<Exchange> Exchange { get; set; }
    }
}
