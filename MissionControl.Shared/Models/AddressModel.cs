using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MissionControl.Shared.Models
{
    public class AddressModel : BaseEntityModel
    {
        public string Address1 { get; set; } = "";

        public string District { get; set; } = "";

        public string City { get; set; } = "";

        public string Province { get; set; } = "";

        public string PostalCode { get; set; } = "";
    }
}
