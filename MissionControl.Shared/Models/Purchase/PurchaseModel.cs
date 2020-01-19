using MissionControl.Shared.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>

    public partial class PurchaseModel : BaseEntityModel
    {
        #region Ctor
        //public PurchaseModel ()
        //{
        //    Vendors = new List<SelectListItem>();
        //    Products = new List<SelectListItem>();
        //}

        #endregion

        #region Properties
        //public IEnumerable<SelectListItem> Vendors { get; set; }
        //public IEnumerable<SelectListItem> Products { get; set; }

        //identifiers

        public int VendorId { get; set; } = 0;

        public string VendorAddress { get; set; } = "";
        public string VendorName { get; set; } = "";
        public decimal PurchaseTotal { get; set; } = 0;
        public string PurchaseTotalValue { get; set; } = "";
        public decimal TotalWeightKg { get; set; }
        public int PurchaseProcessId { get; set; } = 0;

        public string PurchaseProcessIdValue { get; set; } = "";
        public string PurchaseProcessName { get; set; } = "";
        public int PurchaseStatusId { get; set; } = 0;
        public string PurchaseStatusIdValue { get; set; } = "";
        public string PurchaseStatusName { get; set; } = "";
        public int TotalCrates { get; set; } = 0;
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateName { get; set; } = "";
        public string PurchaseNo { get; set; } = "";
        public decimal WeightKg { get; set; } = 0;
        public string Remark { get; set; } = "";
        #endregion


    }


}