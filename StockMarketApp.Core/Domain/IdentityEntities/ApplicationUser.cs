using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace StockMarketApp.Core.Domain.IdentityEntities
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

        public ICollection<BuyOrder> BuyOrders { get; set; }
        public ICollection<SellOrder> SellOrders { get; set; }
    }
}
