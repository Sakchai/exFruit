using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase item model
    /// </summary>
    public partial class PurchaseItemModel : BaseEntityModel
    {
        #region Ctor


        #endregion

        #region Properties

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string VendorName { get; set; } = "";

        public decimal SubTotalExclTax { get; set; } = 0;
        public string SubTotalExclTaxValue { get; set; } = "";
        public decimal SubTotalInclTax { get; set; } = 0;

        public decimal UnitPriceExclTax { get; set; } = 0;
        public string UnitPriceExclTaxValue { get; set; } = "";
        public decimal WeightKg { get; set; } = 0;
        #endregion
        public int PurchaseId { get; set; }

        /// <summary>
        /// Gets or sets the Reception identifier
        /// </summary>
        public int ReceptionId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Sortation identifier
        /// </summary>
        public int SortationId { get; set; } = 0;

        public int ReceivingStatusId { get; set; } = 0;
        public int PurchaseCrates { get; set; }
        public string EAN { get; set; }
        public string ReceivedFruitGradeType { get; set; } = "";
        public decimal ReceivedTotalWeight { get; set; } = 0;
        public int ReceivedCrates { get; set; } = 0;
        public decimal ReceivedCratesWeight { get; set; } = 0;
        public decimal ReceivedActualWeight { get; set; } = 0;
        public string ReceivedNotes { get; set; } = "";
        //public DateTime? ReceivedDateUtc { get; set; }
        //public decimal? SortingWastageBad { get; set; }
        //public int? SortingCrates { get; set; }
        //public string SortingFruitGradeType { get; set; }
        //public string SortingNotes { get; set; }
        //public DateTime? SortingDateUtc { get; set; }
        //public DateTime? CollectionDateUtc { get; set; }
        //public int WarehouseId { get; set; }

  

    }
}
