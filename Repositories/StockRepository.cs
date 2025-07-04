﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class StockRepository : IStockRepository
    {

        private readonly OrdersDbContext _dbContext;

        public StockRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _dbContext.BuyOrders.Add(buyOrder);
            await _dbContext.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _dbContext.SellOrders.Add(sellOrder);
            await _dbContext.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return await _dbContext.BuyOrders.ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return await _dbContext.SellOrders.ToListAsync();
        }
    }
}
