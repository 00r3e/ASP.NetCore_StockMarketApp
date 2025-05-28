using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class OrdersDbContext : DbContext 
    {
        public OrdersDbContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

    }

}
