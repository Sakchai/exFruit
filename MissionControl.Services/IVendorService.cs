using MissionControl.Shared;
using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MissionControl.Services
{
    public interface IVendorService
    {

        IPagedList<Vendor> GetAllVendors(string name = "",int id = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    }
}
