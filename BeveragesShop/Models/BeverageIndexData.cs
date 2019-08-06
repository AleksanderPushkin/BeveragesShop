using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeveragesShop.Models
{
    public class BeverageIndexData
    {
        public IEnumerable<Coins> Coins { get; set; }
        public IEnumerable<Beverages> Beverages { get; set; }
        public Payments Payments { get; set; }
    }
}
