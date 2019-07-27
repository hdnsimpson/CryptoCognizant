using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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

    [DataContract]
    public class CoinDTO
    {
        [DataMember]
        public int CoinId { get; set; }

        [DataMember]
        public string CoinSymbol { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }

        [DataMember]
        public bool IsFavourite { get; set; }
    }

}
