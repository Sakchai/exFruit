using MissionControl.Shared.Models;
using MissionControl.Shared.Models.Common;
using MissionControl.Shared.Models.Purchase;
using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.Services.Factories
{
    public partial interface IPurchaseModelFactory
    {
        PurchaseSearchModel PreparePurchaseSearchModel(PurchaseSearchModel searchModel);

        PurchaseListModel PreparePurchaseListModel(PurchaseSearchRequest searchModel);

        void PreparePurchaseStatuses(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null);
        void PreparePurchaseProcesses(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null);

    }
}
