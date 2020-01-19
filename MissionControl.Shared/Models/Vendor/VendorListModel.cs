using MissionControl.Shared.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace MissionControl.Shared.Models.Vendor
{
    [Serializable]
    [JsonObject]
    public class VendorListModel : CommonListModel
    {
        public IEnumerable<VendorModel> Vendors = new List<VendorModel>();
        public IEnumerable<SelectListItem> TaxTypes = new List<SelectListItem>();
        public IEnumerable<SelectListItem> CompanyTypes = new List<SelectListItem>();


    }
}
