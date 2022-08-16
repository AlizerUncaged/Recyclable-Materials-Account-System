using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recyclable_Materials.Models
{
    public class Transaction
    {
        public long ID { get; set; }

        /// <summary>
        /// The ID of the material in the materials table.
        /// </summary>
        public long Material { get; set; }

        /// <summary>
        /// The ID of the member who created the transaction.
        /// </summary>
        public long Member { get; set; }

        public string Remarks { get; set; }

        public int Quantity { get; set; }

        // Automatically calculated property.
        public double TotalPrice => Quantity * Price;

        public double Price { get; set; }
    }
}
