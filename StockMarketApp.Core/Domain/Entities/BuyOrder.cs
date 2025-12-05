using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockMarketApp.Core.Domain.IdentityEntities;

namespace Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }
        [StringLength(20)]
        [Required(ErrorMessage ="Stock Symbol can't be null or empty")]
        public string StockSymbol { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "Quantity should be between 1 and 100000")]
        public uint Quantity { get; set; }
        [Range(1, 100000, ErrorMessage = "Price should be between 1 and 100000")]
        public double Price { get; set; }

        //FK
        [Required]
        public Guid UserId { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
    }
}
