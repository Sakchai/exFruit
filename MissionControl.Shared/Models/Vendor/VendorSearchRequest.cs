
using MissionControl.Shared.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MissionControl.Shared.Models.Vendor
{
    [Serializable]
    [JsonObject]
    /// <summary>
    /// Represents an vendor model
    /// </summary>
    public partial class VendorSearchRequest
    {


        #region Properties
        public string VendorCode { get; set; } = "";
        public string TaxId { get; set; } = "";
        public string Name { get; set; } = "";
        public string TaxTypeId { get; set; } = "0";
        public string CompanyTypeId { get; set; } = "0";
        public string CountryTypeId { get; set; } = "0";
        #endregion


    }


}