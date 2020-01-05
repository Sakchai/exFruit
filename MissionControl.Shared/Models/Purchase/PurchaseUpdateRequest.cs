using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>

    public partial class PurchaseUpdateRequest: BaseEntityModel
    {

        #region Properties

        public int VendorId { get; set; }
        public string VendorAddress { get; set; }
        public string VendorName { get; set; } = "";
        public string PurchaseStatusIdValue { get; set; } = "";
        public string PurchaseProcessIdValue { get; set; } = "";
        public int TotalCrates { get; set; } = 0;
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateName { get; set; } = "";
        public string PurchaseNo { get; set; } = "";
        public decimal WeightKg { get; set; } = 0;
        public string Remark { get; set; } = "";
        #endregion


    }


}