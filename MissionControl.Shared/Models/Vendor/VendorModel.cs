using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MissionControl.Shared.Models.Vendor
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>
    [Serializable]
    [JsonObject]
    public partial class VendorModel : BaseEntityModel
    {


        #region Properties

        //identifiers
        public string VendorCode { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string TaxID { get; set; } = "";
        public int TaxTypeId { get; set; }
        public string TaxTypeName { get; set; } = "";
        public int CompanyTypeId { get; set; }
        public string CompanyTypeName { get; set; } = "";
        public string Telephone { get; set; } = "";
        public int CountryTypeId { get; set; }
        public string CountryTypeName { get; set; } = "";
        public int AddressId { get; set; }
        public AddressModel Address = new AddressModel();
        #endregion


    }


}