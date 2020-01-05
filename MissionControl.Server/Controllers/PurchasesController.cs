using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MissionControl.Services;
using MissionControl.Services.Common;
using MissionControl.Services.Factories;
using MissionControl.Shared;
using MissionControl.Shared.Models;
using MissionControl.Shared.Models.Common;
using MissionControl.Shared.Models.Purchase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace MissionControl.Server.Controllers
{
    [Route("purchases")]
    [ApiController]
    //[Authorize]
    public class PurchasesController : Controller
    {

        private readonly IPdfService _pdfService;
        private readonly IPurchaseService _purchaseService;
        private readonly IPurchaseModelFactory _purchaseModelFactory;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public PurchasesController(IPurchaseService purchaseService,
            IPdfService pdfService,
            IPurchaseModelFactory purchaseModelFactory,
            IProductService productService,
            IMapper mapper)
        {
            _purchaseService = purchaseService;
            _pdfService = pdfService;
            _purchaseModelFactory = purchaseModelFactory;
            _productService = productService;
            _mapper = mapper;
        }

        //[Authorize]
        //[HttpGet("/listProducts")]
        //public IEnumerable<SelectListItem> ListProducts()
        //{
        //    return SelectListHelper.GetProductList(_productService, false);
        //}

        //[Authorize]
        //[HttpGet("/listPurchaseStatuses")]
        //public IEnumerable<SelectListItem> ListPurchaseStatuses(bool withSpecialDefaultItem, string defaultItemText = "All")
        //{
        //    return SelectListHelper.GetPurchaseStatus(withSpecialDefaultItem, defaultItemText);
        //}

        //[Authorize]
        //[HttpGet("/listPurchaseProcesses")]
        //public IEnumerable<SelectListItem> ListPurchaseProcess(bool withSpecialDefaultItem, string defaultItemText = "All")
        //{
        //    return SelectListHelper.GetPurchaseProcess(withSpecialDefaultItem, defaultItemText);
        //}
        [Authorize]
        [HttpPost("/searchPurchase")]
        public IEnumerable<PurchaseModel> GetPurchases(PurchaseSearchRequest p)
        {
            DateTime? FromDate = null;
            if (!string.IsNullOrWhiteSpace(p.fromPurchaseDate))
            {
                FromDate = Convert.ToDateTime(p.fromPurchaseDate);
            }
            DateTime? ToDate = null;
            if (!string.IsNullOrWhiteSpace(p.toPurchaseDate))
            {
                ToDate = Convert.ToDateTime(p.toPurchaseDate);
            }
            int statusId = 0;
            if (!string.IsNullOrWhiteSpace(p.purchaseStatusId))
            {
                statusId = Int32.Parse(p.purchaseStatusId);
            }
            int processId = 0;
            if (!string.IsNullOrWhiteSpace(p.purchaseProcessId))
            {
                processId = Int32.Parse(p.purchaseProcessId);
            }

            var items = _purchaseService.SearchPurchases(p.vendorName, p.productName, FromDate,
                            ToDate, p.purchaseNo, statusId, processId);

            List<PurchaseModel> purchases = new List<PurchaseModel>();

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

        [Authorize]
        [HttpPost("/updatePurchaseItem")]
        public IEnumerable<PurchaseItemModel> UpdatePurchaseItem(PurchaseItemUpdateRequest req)
        {

            var purchaseItem = _purchaseService.GetPurchaseItemById(req.Id);
            // purchaseItem.ProductId = req.ProductId;
            purchaseItem.ProductName = req.ProductName;
            purchaseItem.PurchaseCrates = req.PurchaseCrates;
            purchaseItem.UnitPriceExclTax = req.UnitPriceExclTax;
            purchaseItem.WeightKg = req.WeightKg;
            _purchaseService.UpdatePurchaseItem(purchaseItem);
            UpdateTotalCratesPurchase(req.PurchaseId);
            return GetPurchaseItemsByPurchaseId(req.PurchaseId);
        }

        [Authorize]
        [HttpPost("/addPurchaseItem")]
        public IEnumerable<PurchaseItemModel> AddPurchaseItem(PurchaseItemUpdateRequest req)
        {
            var purchase = _purchaseService.GetPurchaseById(req.PurchaseId);
            var purchaseItem = new PurchaseItem();
            purchaseItem.Purchase = purchase;
            purchaseItem.PurchaseId = purchase.Id;
            //  purchaseItem.ProductId = req.ProductId;
            purchaseItem.ProductName = req.ProductName;
            purchaseItem.PurchaseCrates = req.PurchaseCrates;
            purchaseItem.UnitPriceExclTax = req.UnitPriceExclTax;
            purchaseItem.WeightKg = req.WeightKg;

            _purchaseService.InsertPurchaseItem(purchaseItem);
            purchaseItem.EAN = CommonUtils.GenerateBarCodeEAN13(purchaseItem.Id);
            _purchaseService.UpdatePurchaseItem(purchaseItem);
            UpdateTotalCratesPurchase(req.PurchaseId);
            return GetPurchaseItemsByPurchaseId(req.PurchaseId);
        }

        [Authorize]
        [HttpPost("/createOrUpdatePurchase")]
        public PurchaseModel CreateOrUpdatePurchase(PurchaseUpdateRequest req)
        {
            var purchase = new Purchase();
            if (req.Id > 0)
                purchase = _purchaseService.GetPurchaseById(req.Id);
            purchase.PurchaseNo = req.PurchaseNo;
            purchase.PurchaseDate = req.PurchaseDate;
            purchase.VendorName = req.VendorName;
            purchase.VendorAddress = req.VendorAddress;
            purchase.PurchaseStatusId = Int32.Parse(req.PurchaseStatusIdValue);
            purchase.PurchaseProcessId = Int32.Parse(req.PurchaseProcessIdValue);
            purchase.TotalCrates = req.TotalCrates;
            purchase.Remark = req.Remark;
            if (req.Id > 0)
                _purchaseService.UpdatePurchase(purchase);
            else
                _purchaseService.InsertPurchase(purchase);
            var purchaseModel = _mapper.Map<PurchaseModel>(purchase);
            return purchaseModel;
        }

        private void UpdateTotalCratesPurchase(int PurchaseId)
        {
            Purchase purchase = _purchaseService.GetPurchaseById(PurchaseId);
            int totalCrates = 0;
            decimal weightKg = 0;
            foreach (var item in _purchaseService.SearchPurchaseItems(PurchaseId))
            {
                totalCrates += item.PurchaseCrates;
                weightKg += item.WeightKg;
            }
            purchase.TotalCrates = totalCrates;
            purchase.TotalWeightKg = weightKg;
            _purchaseService.UpdatePurchase(purchase);

        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<PurchaseModel> GetPurchase([FromRoute] int id)
        {
            if (id == 0)
                return new PurchaseModel();

            var purchase = _purchaseService.GetPurchaseById(id);

            if (purchase == null)
                return new PurchaseModel();

            var p = new PurchaseModel
            {
                Id = purchase.Id,
                PurchaseNo = purchase.PurchaseNo,
                PurchaseStatusId = purchase.PurchaseStatusId == null ? 0 : purchase.PurchaseStatusId.Value,
                PurchaseStatusIdValue = purchase.PurchaseStatusId.Value.ToString(),
                PurchaseStatusName = purchase.PurchaseStatus.ToString(),
                VendorName = (purchase.Vendor == null) ? purchase.VendorName : purchase.Vendor.Name,
                VendorAddress = purchase.VendorAddress,
                TotalCrates = purchase.TotalCrates,
                PurchaseDate = (purchase.PurchaseDate == null) ? DateTime.Today : purchase.PurchaseDate.Value,
                PurchaseDateName = purchase.PurchaseDate.HasValue ? purchase.PurchaseDate.Value.ToString("yyyy-MM-dd") : "N/A",
                Remark = purchase.Remark,
            };

            return p;

        }

        [HttpGet("/purchaseBarcode/{id}")]
        public ActionResult<string> GetBarcode([FromRoute] int id)
        {

            return (id == 0) ? string.Empty : _pdfService.PrintBarcodeToPdf(id);
        }

        [Authorize]
        [HttpGet("/purchaseItems/{id}")]
        public IEnumerable<PurchaseItemModel> GetAllPurchaseItems(int id)
        {
            if (id == 0)
                return new List<PurchaseItemModel>();

            return GetPurchaseItemsByPurchaseId(id);
        }


        [Authorize]
        [HttpGet("/deletePurchaseItem/{id}")]
        public IEnumerable<PurchaseItemModel> DeletePurchaseItem(int id)
        {
            if (id == 0)
                return new List<PurchaseItemModel>();
            var purchaseItem = _purchaseService.GetPurchaseItemById(id);
            int purchaseId = purchaseItem.PurchaseId;
            _purchaseService.DeletePurchaseItem(id);
            UpdateTotalCratesPurchase(purchaseId);
            return GetPurchaseItemsByPurchaseId(purchaseId);

        }

        private List<PurchaseItemModel> GetPurchaseItemsByPurchaseId(int id)
        {
            var items = new List<PurchaseItemModel>();

            var purchaseItems = _purchaseService.SearchPurchaseItems(id);

            if (purchaseItems == null)
                return items;

            foreach (var item in purchaseItems)
            {
                items.Add(new PurchaseItemModel
                {
                    Id = item.Id,
                    PurchaseId = item.PurchaseId,
                    EAN = item.EAN,
                    ProductName = item.ProductName,
                    WeightKg = item.WeightKg,
                    UnitPriceExclTax = item.UnitPriceExclTax,
                    UnitPriceExclTaxValue = item.UnitPriceExclTax.ToString("N", CultureInfo.InvariantCulture),
                    PurchaseCrates = item.PurchaseCrates,
                    SubTotalExclTax = item.SubTotalExclTax,
                    SubTotalExclTaxValue = item.SubTotalExclTax.ToString("N", CultureInfo.InvariantCulture),
                });
            }
            return items;
        }



        //[HttpGet("/purchaseItem/{id}")]
        //public async Task<ActionResult<List<PurchaseItem>>> GetPurchaseItem(int id)
        //{
        //    var purchaseItems = await _db.PurchaseItems
        //        .Where(o => o.Id == id)
        //        .Include(p => p.Product)
        //        .ToListAsync();

        //    return purchaseItems;
        //}

        //[HttpPost("/savePurchaseItem")]
        //public async Task<ActionResult<int>> SavePurchaseItems([FromRoute] PurchaseItemModel purchaseItemModel)
        //{
        //    // Enforce existence of Pizza.SpecialId and Topping.ToppingId
        //    // in the database - prevent the submitter from making up
        //    // new specials and toppings
        //    PurchaseItem purchaseItem = NewPurchaseItem(purchaseItemModel);
        //    _db.PurchaseItems.Attach(purchaseItem);
        //    await _db.SaveChangesAsync();

        //    // In the background, send push notifications if possible
        //    var subscription = await _db.NotificationSubscriptions.Where(e => e.UserId == GetUserId()).SingleOrDefaultAsync();
        //    if (subscription != null)
        //    {
        //        _ = TrackAndSendNotificationsItemAsync(purchaseItem, subscription);
        //    }

        //    return purchaseItem.Id;
        //}

        //private static PurchaseItem NewPurchaseItem(PurchaseItemModel purchaseItemModel)
        //{
        //    return new PurchaseItem
        //    {
        //        ProductId = purchaseItemModel.ProductId,
        //    };
        //}

        //[HttpPost]
        //public async Task<ActionResult<int>> SavePurchase([FromRoute] PurchaseModel purchaseModel)
        //{
        //    // Enforce existence of Pizza.SpecialId and Topping.ToppingId
        //    // in the database - prevent the submitter from making up
        //    // new specials and toppings
        //    Purchase purchase = NewPurchase(purchaseModel);
        //    _db.Purchases.Attach(purchase);
        //    await _db.SaveChangesAsync();

        //    // In the background, send push notifications if possible
        //    var subscription = await _db.NotificationSubscriptions.Where(e => e.UserId == GetUserId()).SingleOrDefaultAsync();
        //    if (subscription != null)
        //    {
        //        _ = TrackAndSendNotificationsAsync(purchase, subscription);
        //    }

        //    return purchase.Id;
        //}

        //private static Purchase NewPurchase(PurchaseModel purchaseModel)
        //{
        //    return new Purchase
        //    {
        //        VendorId = purchaseModel.VendorId,
        //    };
        //}

        //// DELETE: api/Purchase/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePurchase([FromRoute] int id)
        //{
        //    var purchase = await _db.Purchases.SingleOrDefaultAsync(p => p.Id == id);
        //    if (purchase == null)
        //        return NotFound();
        //    purchase.Deleted = true;
        //    _db.Purchases.Update(purchase);
        //    await _db.SaveChangesAsync();
        //    return Ok(purchase);
        //}

        //[HttpDelete("/deletePurchaseItem/{id}")]
        //public async Task<IActionResult> DeletePurchaseItem([FromRoute] int id)
        //{
        //    var purchaseItem = await _db.PurchaseItems.SingleOrDefaultAsync(p => p.Id == id);
        //    if (purchaseItem == null)
        //        return NotFound();
        //    _db.PurchaseItems.Remove(purchaseItem);
        //    await _db.SaveChangesAsync();
        //    return Ok(purchaseItem);
        //}
        //private string GetUserId()
        //{
        //    // This will be the user's twitter username
        //    return HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        //}

        //private static async Task TrackAndSendNotificationsAsync(Purchase purchase, NotificationSubscription subscription)
        //{
        //    // In a realistic case, some other backend process would track
        //    // purchase delivery progress and send us notifications when it
        //    // changes. Since we don't have any such process here, fake it.
        //    //await Task.Delay(PurchaseItem.PreparationDuration);
        //    //await SendNotificationAsync(purchase, subscription, "Your purchase has been dispatched!");

        //    //await Task.Delay(PurchaseItem.DeliveryDuration);
        //    //await SendNotificationAsync(purchase, subscription, "Your purchase is now delivered. Enjoy!");
        //}
        //private static async Task TrackAndSendNotificationsItemAsync(PurchaseItem purchaseItem, NotificationSubscription subscription)
        //{
        //    // In a realistic case, some other backend process would track
        //    // purchase delivery progress and send us notifications when it
        //    // changes. Since we don't have any such process here, fake it.
        //    //await Task.Delay(PurchaseItem.PreparationDuration);
        //    //await SendNotificationAsync(purchase, subscription, "Your purchase has been dispatched!");

        //    //await Task.Delay(PurchaseItem.DeliveryDuration);
        //    //await SendNotificationAsync(purchase, subscription, "Your purchase is now delivered. Enjoy!");
        //}

        //private static async Task SendNotificationAsync(Purchase purchase, NotificationSubscription subscription, string message)
        //{
        //    // For a real application, generate your own
        //    var publicKey = "BLC8GOevpcpjQiLkO7JmVClQjycvTCYWm6Cq_a7wJZlstGTVZvwGFFHMYfXt6Njyvgx_GlXJeo5cSiZ1y4JOx1o";
        //    var privateKey = "OrubzSz3yWACscZXjFQrrtDwCKg-TGFuWhluQ2wLXDo";

        //    var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
        //    var vapidDetails = new VapidDetails("mailto:<someone@example.com>", publicKey, privateKey);
        //    var webPushClient = new WebPushClient();
        //    try
        //    {
        //        var payload = JsonSerializer.Serialize(new
        //        {
        //            message,
        //            url = $"mypurchases/{purchase.Id}",
        //        });
        //        await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.WriteLine("Error sending push notification: " + ex.Message);
        //    }
        //}
    }
}
