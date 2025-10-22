using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StockMarketApp.Core.Domain.IdentityEntities
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
