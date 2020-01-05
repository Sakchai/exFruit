using MissionControl.Shared;
using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControl.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IRepository<Purchase> _purchaseRepository;
        private readonly IRepository<PurchaseItem> _purchaseItemRepository;
        public PurchaseService(IRepository<Purchase> purchaseRepository,
            IRepository<PurchaseItem> purchaseItemRepository)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemRepository = purchaseItemRepository;
        }
        public IPagedList<Purchase> SearchPurchases(string vendorName = "", string productName = "", DateTime? dateFrom = null, DateTime? dateTo = null, string purchaseNumber = "", int purchaseStatusId = 0, int purchaseProcessId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _purchaseRepository.Table;
            if (!string.IsNullOrWhiteSpace(productName))
                query = query.Where(o => o.PurchaseItems.Any(i => i.ProductName == productName));

            if (!string.IsNullOrWhiteSpace(vendorName))
                query = query.Where(o => o.Vendor.Name.Contains(vendorName));

            if (dateFrom.HasValue)
                query = query.Where(o => dateFrom.Value <= o.PurchaseDate);

            if (dateTo.HasValue)
                query = query.Where(o => dateTo.Value >= o.PurchaseDate);

            if (!string.IsNullOrWhiteSpace(purchaseNumber))
                query = query.Where(o => o.PurchaseNo == purchaseNumber);

            if(purchaseStatusId > 0)
                query = query.Where(o => o.PurchaseStatusId == purchaseStatusId);

            if (purchaseProcessId > 0)
                query = query.Where(o => o.PurchaseProcessId == purchaseProcessId);

            query = query.Where(o => !o.Deleted);

            query = query.OrderByDescending(o => o.CreatedOnUtc);

            return new PagedList<Purchase>(query, pageIndex, pageSize);
        }

        public virtual void InsertPurchase(Purchase purchase)
        {
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase));

            _purchaseRepository.Insert(purchase);

        }

        public virtual void UpdatePurchase(Purchase purchase)
        {
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase));

            _purchaseRepository.Update(purchase);
        }

        public virtual void DeletePurchase(Purchase purchase)
        {
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase));

            purchase.Deleted = true;
            _purchaseRepository.Update(purchase);
        }

        public Purchase GetPurchaseById(int purchaseId)
        {
            if (purchaseId == 0)
                return null;

            return _purchaseRepository.GetById(purchaseId);
        }

        public IPagedList<PurchaseItem> SearchPurchaseItems(int purchaseId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _purchaseItemRepository.Table;
            if (purchaseId > 0)
                query = query.Where(x => x.PurchaseId == purchaseId);

            query = query.OrderBy(x => x.Id);

            return new PagedList<PurchaseItem>(query, pageIndex, pageSize);
        }

        public void DeletePurchaseItem(int Id)
        {
            var item = _purchaseItemRepository.GetById(Id);
            _purchaseItemRepository.Delete(item);

        }

        public void UpdatePurchaseItem(PurchaseItem item)
        {
            _purchaseItemRepository.Update(item);
        }

        public void InsertPurchaseItem(PurchaseItem item)
        {
            _purchaseItemRepository.Insert(item);
        }

        public PurchaseItem GetPurchaseItemById(int id)
        {
            if (id == 0)
                return null;

            return _purchaseItemRepository.GetById(id);
        }
    }
}
