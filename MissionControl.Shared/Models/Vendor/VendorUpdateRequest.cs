using MissionControl.Shared.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MissionControl.Shared.Models.Vendor
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>

    public partial class VendorUpdateRequest : VendorModel
    {

        #region Properties


        public string Address1 { get; set; }

        public string Distinct { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        #endregion


    }


}