using System;
using System.Collections.Generic;

namespace BeveragesShop.Models
{
    public partial class Producers
    {
        public Producers()
        {
            Beverages = new HashSet<Beverages>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Beverages> Beverages { get; set; }
    }
}
