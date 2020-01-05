
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

        public string fromPurchaseDate { get; set; } = "";
        public string toPurchaseDate { get; set; } = "";

        public string purchaseStatusId { get; set; } = "0";

        public string purchaseProcessId { get; set; } = "0";

        public string productName { get; set; } = "";
        public string purchaseNo { get; set; } = "";
        public string vendorName { get; set; } = "";

        #endregion


    }


}