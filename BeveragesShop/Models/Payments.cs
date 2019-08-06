using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeveragesShop.Models
{
    public partial class Payments
    {
        public int Id { get; set; }
        public int BeverageId { get; set; }
        public int Count { get; set; }
        public int Money { get; set; }
        public int Change { get; set; }
        public int Price { get; set; }



        [NotMapped]
        public List<Coins> Coins { get; set; }

        [NotMapped]
        public List<Beverages> Beverages { get; set; }
    }
}
