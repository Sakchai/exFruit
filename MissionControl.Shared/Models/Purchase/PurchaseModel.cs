using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>

    public partial class PurchaseModel : BaseEntityModel
    {
        #region Ctor


        #endregion

        #region Properties



        //identifiers

        public int VendorId { get; set; }
        public string VendorFullName { get; set; } = "";
        public decimal PurchaseTotal { get; set; } = 0;
        public string PurchaseTotalValue { get; set; } = "";
        public int PurchaseProcessId { get; set; } = 0;

        public string PurchaseProcessIdValue { get; set; } = "";
        public string PurchaseProcessName { get; set; } = "";
        public int PurchaseStatusId { get; set; } = 0;
        public string PurchaseStatusIdValue { get; set; } = "";
        public string PurchaseStatusName { get; set; } = "";
        public int TotalCrates { get; set; } = 0;
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateName { get; set; } = "";
        public string PurchaseNo { get; set; } = "";
        public decimal WeightKg { get; set; } = 0;

        #endregion


    }


}