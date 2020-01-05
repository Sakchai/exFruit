using System;
using System.Collections.Generic;

namespace MissionControl.Shared
{
    /// <summary>
    /// Represents an order item
    /// </summary>
    public partial class PurchaseItem : BaseEntity
    {
        //public int Id { get; set; }

        /// <summary>
        /// Gets or sets the order item identifier
        /// </summary>
        public Guid PurchaseItemGuid { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int PurchaseId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (include tax)
        /// </summary>
        public decimal UnitPriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (exclude tax)
        /// </summary>
        public decimal UnitPriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (include tax)
        /// </summary>
        public decimal PriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (exclude tax)
        /// </summary>
        public decimal PriceExclTax { get; set; }

 
        /// <summary>
        /// Gets or sets the total weight of one item
        /// It's nullable for compatibility with the previous version of nopCommerce where was no such property
        /// </summary>
        public decimal? ItemWeight { get; set; }


        /// <summary>
        /// Gets or sets the Reception identifier
        /// </summary>
        public int ReceptionId { get; set; }

        /// <summary>
        /// Gets or sets the Sortation identifier
        /// </summary>
        public int SortationId { get; set; }

        public string ProductName { get; set; }
        public int ReceivingStatusId { get; set; }
        public int PurchaseCrates { get; set; }
        public string EAN { get; set; }
        public string ReceivedFruitGradeType { get; set; }
        public decimal? ReceivedTotalWeight { get; set; }
        public int ReceivedCrates { get; set; }
        public decimal? ReceivedCratesWeight { get; set; }
        public decimal? ReceivedActualWeight { get; set; }
        public string ReceivedNotes { get; set; }
        public DateTime? ReceivedDateUtc { get; set; }
        public decimal? SortingWastageBad { get; set; }
        public int? SortingCrates { get; set; }
        public string SortingFruitGradeType { get; set; }
        public string SortingNotes { get; set; }
        public DateTime? SortingDateUtc { get; set; }
        public DateTime? CollectionDateUtc { get; set; }
        public int WarehouseId { get; set; }

        public decimal WeightKg { get; set; }
        public decimal SubTotalExclTax { get; set; }
        /// <summary>
        /// Gets the order
        /// </summary>
        public virtual Purchase Purchase { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets the Reception
        /// </summary>
        public virtual Reception Reception { get; set; }


        /// <summary>
        /// Gets the Sortation
        /// </summary>
        public virtual Sortation Sortation { get; set; }

        public string Key => $"{EAN}:{Id}";

    }
}
