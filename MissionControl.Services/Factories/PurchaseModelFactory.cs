using AutoMapper;
using MissionControl.Shared;
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

        public PurchaseListModel PreparePurchaseListModel(PurchaseSearchRequest p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            DateTime? FromDate = null;
            if (!string.IsNullOrWhiteSpace(p.FromPurchaseDate))
            {
                FromDate = Convert.ToDateTime(p.FromPurchaseDate);
            }
            DateTime? ToDate = null;
            if (!string.IsNullOrWhiteSpace(p.ToPurchaseDate))
            {
                ToDate = Convert.ToDateTime(p.ToPurchaseDate);
            }
            int statusId = 0;
            if (!string.IsNullOrWhiteSpace(p.PurchaseStatusId))
            {
                statusId = Int32.Parse(p.PurchaseStatusId);
            }
            int processId = 0;
            if (!string.IsNullOrWhiteSpace(p.PurchaseProcessId))
            {
                processId = Int32.Parse(p.PurchaseProcessId);
            }

            var items = _purchaseService.SearchPurchases(p.VendorName, p.ProductName, FromDate,
                            ToDate, p.PurchaseNo, statusId, processId);

            var purchases = CreatePurchaseModelList(items);

            var purchaseListModel = new PurchaseListModel();
            purchaseListModel.Purchases = purchases;
            purchaseListModel.HasNextPage = items.HasNextPage;
            purchaseListModel.HasPreviousPage = items.HasPreviousPage;
            purchaseListModel.PageIndex = items.PageIndex;
            purchaseListModel.PageSize = items.PageSize;
            purchaseListModel.TotalCount = items.TotalCount;
            purchaseListModel.TotalPages = items.TotalPages;
            purchaseListModel.PurchaseStatus = p.PurchaseStatusId.Equals("0") ? SelectListHelper.GetPurchaseStatus(false, "ALL")
                                                : SelectListHelper.GetPurchaseStatus(true, p.PurchaseStatusName);
            purchaseListModel.PurchaseProcessStatus = p.PurchaseProcessId.Equals("0") ? SelectListHelper.GetPurchaseProcess(false, "ALL")
                                                : SelectListHelper.GetPurchaseProcess(true, p.PurchaseProcessName);
            return purchaseListModel;
        }

        private static List<PurchaseModel> CreatePurchaseModelList(IPagedList<Purchase> items)
        {
            var purchases = new List<PurchaseModel>();

            foreach (var item in items)
            {

                purchases.Add(new PurchaseModel
                {
                    Id = item.Id,
                    PurchaseNo = item.PurchaseNo,
                    PurchaseStatusName = item.PurchaseStatus.ToString(),
                    PurchaseProcessName = item.PurchaseProcess.ToString(),
                    VendorName = (item.Vendor == null) ? item.VendorName : item.Vendor.Name,
                    TotalCrates = item.TotalCrates,
                    PurchaseDateName = item.PurchaseDate.HasValue ? item.PurchaseDate.Value.ToString("yyyy-MM-dd") : "N/A",
                    Remark = item.Remark,
                });
            }

            return purchases;
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
