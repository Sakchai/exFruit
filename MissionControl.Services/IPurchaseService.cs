using MissionControl.Shared;
using MissionControl.Shared.Models.Common;
using System;

namespace MissionControl.Services
{
    public interface IPurchaseService
    {

        IPagedList<Purchase> SearchPurchases(string vendorName = "", string productName = "",
            DateTime? dateFrom = null, DateTime? dateTo = null,
            string purchaseNumber = "", int purchaseStatusId = 0, int purchaseProcessId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<PurchaseItem> SearchPurchaseItems(int purchaseId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);


        void InsertPurchase(Purchase purchase);

        void UpdatePurchase(Purchase purchase);

        void DeletePurchase(Purchase purchase);

        void DeletePurchaseItem(int Id);

        Purchase GetPurchaseById(int purchaseId);
    }
}
