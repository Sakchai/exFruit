using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace MissionControl.Shared.Models.Purchase
{
  
    public class PurchaseListModel : CommonListModel
    {

        public IEnumerable<PurchaseModel> Purchases = new List<PurchaseModel>();
        public IEnumerable<SelectListItem> PurchaseStatus = new List<SelectListItem>();
        public IEnumerable<SelectListItem> PurchaseProcessStatus = new List<SelectListItem>();

    }
}
