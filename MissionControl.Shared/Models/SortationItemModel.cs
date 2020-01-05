using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase item model
    /// </summary>
    public partial class SortationItemModel : BaseEntityModel
    {
        #region Ctor


        #endregion

        #region Properties

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string VendorName { get; set; }

        public string Sku { get; set; }

        public string PictureThumbnailUrl { get; set; }

        public string UnitPriceInclTax { get; set; }

        public string UnitPriceExclTax { get; set; }

        public decimal UnitPriceInclTaxValue { get; set; }

        public decimal UnitPriceExclTaxValue { get; set; }

        public int Quantity { get; set; }

        public string DiscountInclTax { get; set; }

        public string DiscountExclTax { get; set; }

        public decimal DiscountInclTaxValue { get; set; }

        public decimal DiscountExclTaxValue { get; set; }

        public string SubTotalInclTax { get; set; }

        public string SubTotalExclTax { get; set; }

        public decimal SubTotalInclTaxValue { get; set; }

        public decimal SubTotalExclTaxValue { get; set; }


        public decimal WeightKg { get; set; }
        #endregion


        /// <summary>
        /// Gets or sets the Reception identifier
        /// </summary>
        public int ReceptionId { get; set; }

        /// <summary>
        /// Gets or sets the Sortation identifier
        /// </summary>
        public int SortationId { get; set; }

        public int ReceivingStatusId { get; set; }
        public int PurchaseCrates { get; set; }
        public string EAN { get; set; }

        public DateTime? ReceivedDateUtc { get; set; }
        public decimal? SortingWastageBad { get; set; }
        public int? SortingCrates { get; set; }
        public string SortingFruitGradeType { get; set; }
        public string SortingNotes { get; set; }
        public DateTime? SortingDateUtc { get; set; }
        //public DateTime? CollectionDateUtc { get; set; }
        public int WarehouseId { get; set; }

        ///// <summary>
        ///// Gets the order
        ///// </summary>
        //public virtual Purchase Purchase { get; set; }

        ///// <summary>
        ///// Gets the product
        ///// </summary>
        //public virtual Product Product { get; set; }

        ///// <summary>
        ///// Gets the Reception
        ///// </summary>
        //public virtual Reception Reception { get; set; }


        ///// <summary>
        ///// Gets the Sortation
        ///// </summary>
        //public virtual Sortation Sortation { get; set; }

    }
}
