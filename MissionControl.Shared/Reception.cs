using System;
using System.Collections.Generic;

namespace MissionControl.Shared
{
    /// <summary>
    /// Represents a reception
    /// </summary>
    public partial class Reception : BaseEntity
    {
        //public int Id { get; set; }

        private ICollection<PurchaseItem> _purchaseItems;


        /// <summary>
        /// Gets or sets the Received date and time
        /// </summary>
        public DateTime? ReceivedDateUtc { get; set; }

        public string ReceivedBy { get; set; }
        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets the entity creation date
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the reception items
        /// </summary>
        public virtual ICollection<PurchaseItem> PurchaseItems
        {
            get => _purchaseItems ?? (_purchaseItems = new List<PurchaseItem>());
            protected set => _purchaseItems = value;
        }


    }
}