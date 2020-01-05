using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase item model
    /// </summary>
    public partial class ReceptionItemModel : BaseEntityModel
    {
        #region Ctor


        #endregion

        #region Properties

        public int ProductId { get; set; }

        public string ProductName { get; set; }

    
        public int Quantity { get; set; }

     
        public decimal WeightKg { get; set; }


        /// <summary>
        /// Gets or sets the Reception identifier
        /// </summary>
        public int ReceptionId { get; set; }

        public int ReceivingStatusId { get; set; }
        public int PurchaseCrates { get; set; }
        public string EAN { get; set; }
        public string ReceivedFruitGradeType { get; set; }
        public decimal? ReceivedTotalWeight { get; set; }
        public int ReceivedCrates { get; set; }
        public decimal? ReceivedCratesWeight { get; set; }
        public decimal? ReceivedActualWeight { get; set; }
        public string ReceivedNotes { get; set; }

        #endregion

    }
}
