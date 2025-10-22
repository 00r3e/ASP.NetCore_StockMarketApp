using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMarketApp.Core.Domain.IdentityEntities;

namespace Entities
{
    public class OrdersDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public OrdersDbContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

    }

}
