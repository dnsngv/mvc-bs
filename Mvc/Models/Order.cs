using System;
using System.Collections.Generic;

namespace Mvc.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderEntry> Entries { get; set; } = new ();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
