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
using System.IO;
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
        private readonly IVendorService _vendorService;
        private readonly IMapper _mapper;

        public PurchasesController(IPurchaseService purchaseService,
            IVendorService vendorService,
            IPdfService pdfService,
            IPurchaseModelFactory purchaseModelFactory,
            IProductService productService,
            IMapper mapper)
        {
            _purchaseService = purchaseService;
            _pdfService = pdfService;
            _purchaseModelFactory = purchaseModelFactory;
            _productService = productService;
            _vendorService = vendorService;

            _mapper = mapper;
        }


        [Authorize]
        [HttpPost("/searchPurchaseList")]
        public PurchaseListModel SearchPurchaseList(PurchaseSearchRequest p)
        {
            return _purchaseModelFactory.PreparePurchaseListModel(p);
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
            purchaseItem.ProductId = req.ProductId;
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
            purchase.PurchaseDate = req.PurchaseDate.Date;
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

        // [Authorize]
        [HttpGet("{id}")]
        public ActionResult<PurchaseModel> GetPurchase([FromRoute] int id)
        {
            var purchaseModel = new PurchaseModel();
            //purchaseModel.Products = SelectListHelper.GetProductList(_productService, false);
            //purchaseModel.Vendors = SelectListHelper.GetVendorList(_vendorService, false);
            if (id == 0)
                return purchaseModel;

            var purchase = _purchaseService.GetPurchaseById(id);

            if (purchase == null)
                return purchaseModel;
            purchaseModel.Id = purchase.Id;
            purchaseModel.PurchaseNo = purchase.PurchaseNo;
            purchaseModel.PurchaseStatusId = purchase.PurchaseStatusId == null ? 0 : purchase.PurchaseStatusId.Value;
            purchaseModel.PurchaseStatusIdValue = purchase.PurchaseStatusId.Value.ToString();
            purchaseModel.PurchaseStatusName = purchase.PurchaseStatus.ToString();
            purchaseModel.VendorName = (purchase.Vendor == null) ? purchase.VendorName : purchase.Vendor.Name;
            purchaseModel.VendorAddress = purchase.VendorAddress;
            purchaseModel.TotalCrates = purchase.TotalCrates;
            purchaseModel.PurchaseDate = (purchase.PurchaseDate == null) ? DateTime.Today : purchase.PurchaseDate.Value;
            purchaseModel.PurchaseDateName = purchase.PurchaseDate.HasValue ? purchase.PurchaseDate.Value.ToString("yyyy-MM-dd") : "N/A";
            purchaseModel.Remark = purchase.Remark;
            return purchaseModel;

        }

        [HttpGet("/purchaseBarcode/{id}")]
        public ActionResult GetBarcode([FromRoute] int id)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _pdfService.PrintBarcodeToPdf(stream, id);
                bytes = stream.ToArray();
            }

            string guid = string.Format("{0,10:D6}", id).Trim();
            var fileName = $"barcode_{guid}_{GenerateRandomDigitCode(2)}.pdf";
            return File(bytes, "application/pdf", fileName);
        }

        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (var i = 0; i < length; i++)
                str = string.Concat(str, random.Next(10).ToString());
            return str;
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


    }
}
