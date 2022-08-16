using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recyclable_Materials.Models
{
    /// <summary>
    /// A structure representing a material.
    /// </summary>
    public class Material
    {
        public Material(long id, string name, double quantity)
        {
            Name = name;
            Quantity = quantity;
            ID = id;
            Biodegradable = false;
        } 
        
        public Material()
        {
            
        }

        public long ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The number of occurences the material has been used in the table.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Describes if the material is Biodegradable, if false it's recyclable.
        /// </summary>
        public bool Biodegradable { get; set; }
    }
}
