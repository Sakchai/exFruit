using Microsoft.EntityFrameworkCore;
using MissionControl.Shared;
using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControl.Services
{
    public class VendorService :IVendorService
    {
        private readonly IRepository<Vendor> _vendorRepository;
        public VendorService(IRepository<Vendor> vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }



        public IPagedList<Vendor> GetAllVendors(string name = "",int id =0, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _vendorRepository.Table;
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));

            if (id > 0)
                query = query.Where(x => x.Id == id);

            var vendors = new PagedList<Vendor>(query, pageIndex, pageSize);
            return vendors;
        }


    }
}
