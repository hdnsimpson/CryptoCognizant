using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCognizant.Model
{
    public partial class Exchange
    {
        public int ExchangeId { get; set; }
        [Required]
        [StringLength(255)]
        public string ExchangeName { get; set; }
        public bool IsActive { get; set; }
        [StringLength(1000)]
        public string Pairs { get; set; }
    }
}
