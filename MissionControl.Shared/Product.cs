using System;
using System.Collections.Generic;
using System.Linq;


namespace MissionControl.Shared
{
    /// <summary>
    /// Represents a product
    /// </summary>
    public partial class Product : BaseEntity
    {

        //public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        public string FullDescription { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
    }
}