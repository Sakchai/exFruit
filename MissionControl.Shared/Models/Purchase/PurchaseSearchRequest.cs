
using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;

namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>
    public partial class PurchaseSearchRequest
    {
        #region Ctor

        #endregion

        #region Properties

        public string FromPurchaseDate { get; set; } = "";
        public string ToPurchaseDate { get; set; } = "";

        public string PurchaseStatusId { get; set; } = "0";
        public string PurchaseStatusName { get; set; } = "";

        public string PurchaseProcessId { get; set; } = "0";
        public string PurchaseProcessName { get; set; } = "";

        public string ProductName { get; set; } = "";
        public string PurchaseNo { get; set; } = "";
        public string VendorName { get; set; } = "";
        public string VenderId { get; set; } = "0";
        public string ProductId { get; set; } = "0";
        #endregion


    }


}