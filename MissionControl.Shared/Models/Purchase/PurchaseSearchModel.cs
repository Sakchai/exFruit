
using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;

namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>
    public partial class PurchaseSearchModel : PurchaseSearchRequest
    {
        #region Ctor

        public PurchaseSearchModel()
        {
          //  Items = new List<PurchaseItemModel>();
            AvailablePurchaseStatuses = new List<SelectListItem>();
            AvailablePurchaseProcesses = new List<SelectListItem>();
            AvailableProducts = new List<SelectListItem>();
        }

        #endregion

        #region Properties
        public IList<SelectListItem> AvailablePurchaseStatuses { get; set; }
        public IList<SelectListItem> AvailablePurchaseProcesses { get; set; }
        public IList<SelectListItem> AvailableProducts { get; set; }
        //PurchaseDate

        #endregion


    }


}