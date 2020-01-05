using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.Shared.Models.Purchase
{
    public class PurchaseItemUpdateRequest : BaseEntityModel
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal WeightKg { get; set; }

        public decimal UnitPriceExclTax { get; set; }

        public int PurchaseCrates { get; set; }
    }
}
