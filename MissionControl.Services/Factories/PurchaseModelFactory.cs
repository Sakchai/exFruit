using AutoMapper;
using MissionControl.Shared.Enum;
using MissionControl.Shared.Models;
using MissionControl.Shared.Models.Common;
using MissionControl.Shared.Models.Purchase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MissionControl.Services.Factories
{
    public class PurchaseModelFactory :IPurchaseModelFactory
    {
        private readonly IProductService _productService;
        private readonly IPurchaseService _purchaseService;
        public PurchaseModelFactory(IProductService productService,
            IPurchaseService purchaseService)
        {
            _productService = productService;
            _purchaseService = purchaseService;
        }

        public PurchaseListModel PreparePurchaseListModel(PurchaseSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            DateTime fromDate = DateTime.ParseExact(searchModel.fromPurchaseDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(searchModel.toPurchaseDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);

            var items = _purchaseService.SearchPurchases(searchModel.vendorName, searchModel.productName, fromDate,
                 toDate, searchModel.vendorName, Int32.Parse(searchModel.purchaseStatusId), Int32.Parse(searchModel.purchaseProcessId));

            var purchaseModels = new List<PurchaseModel>(); 

            foreach (var item in items)
            {
                purchaseModels.Add(new PurchaseModel
                {
                    Id = item.Id,
                    PurchaseStatusName = item.PurchaseStatus.ToString(),
                    PurchaseProcessName = item.PurchaseProcess.ToString(),
                    VendorName = (item.Vendor == null) ? item.VendorName :  item.Vendor.Name,
                    TotalCrates = item.TotalCrates,
                }); 
            }
            var purchaseListModel = new PurchaseListModel();
            purchaseListModel.PurchaseModel = purchaseModels.ToArray();
            return purchaseListModel;
        }

        public PurchaseSearchModel PreparePurchaseSearchModel(PurchaseSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            PreparePurchaseStatuses(searchModel.AvailablePurchaseStatuses);
            PreparePurchaseProcesses(searchModel.AvailablePurchaseProcesses);
            PrepareProducts(searchModel.AvailableProducts);
            return searchModel;
        }

        public void PreparePurchaseStatuses(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available return request statuses
            var availableStatusItems = PurchaseStatus.Pending.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PreparePurchaseProcesses(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available return request statuses
            var availableStatusItems = PurchaseProcess.Purchase.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        protected virtual void PrepareDefaultItem(IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = null)
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

        public virtual void PrepareProducts(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available vendors
            var availableProductItems = SelectListHelper.GetProductList(_productService, true);
            foreach (var item in availableProductItems)
            {
                items.Add(item);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }
    }
}
