using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MissionControl.Shared.Enum;

namespace MissionControl.Services
{
    public static class SelectListHelper
    {
        public static List<SelectListItem> GetProductList(IProductService productService, bool showHidden = false)
        {
            if (productService == null)
                throw new ArgumentNullException(nameof(productService));

            var products = productService.GetAllProducts();
            var items = new List<SelectListItem>();
            foreach (var item in products)
            {
                items.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            };
            return items;

        }

        public static List<SelectListItem> GetPurchaseStatus(bool withSpecialDefaultItem, string defaultItemText = null)
        {
            var items = new List<SelectListItem>();
            //prepare available return request statuses
            var availableStatusItems = PurchaseStatus.Pending.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
            return items;
        }

        public static List<SelectListItem> GetPurchaseProcess(bool withSpecialDefaultItem, string defaultItemText = null)
        {
            var items = new List<SelectListItem>();
            //prepare available return request statuses
            var availableStatusItems = PurchaseProcess.Purchase.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
            return items;
        }

        private static void PrepareDefaultItem(IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //at now we use "0" as the default value
            const string value = "0";

            //prepare item text
            defaultItemText = "All";

            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = value });
        }
    }
}
