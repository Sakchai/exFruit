using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MissionControl.Services;
using MissionControl.Services.Common;
using MissionControl.Shared;
using MissionControl.Shared.Models;
using MissionControl.Shared.Models.Common;
using MissionControl.Shared.Models.Vendor;
using System.Collections.Generic;

namespace MissionControl.Server.Controllers
{
    [Route("vendors")]
    [ApiController]
    //[Authorize]
    public class VendorsController : Controller
    {

        private readonly IVendorService _vendorService;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public VendorsController(IVendorService vendorService,
            IAddressService addressService,
            IMapper mapper)
        {

            _vendorService = vendorService;
            _addressService = addressService;
            _mapper = mapper;
        }

   

        [Authorize]
        [HttpPost("/searchVendorList")]
        public VendorListModel SearchVendorList(VendorSearchRequest p)
        {
            var items = _vendorService.GetAllVendors(p.Name);

            var vendors = CreateVendorModelList(items);

            var vendorListModel = new VendorListModel();
            vendorListModel.Vendors = vendors;
            vendorListModel.HasNextPage = items.HasNextPage;
            vendorListModel.HasPreviousPage = items.HasPreviousPage;
            vendorListModel.PageIndex = items.PageIndex;
            vendorListModel.PageSize = items.PageSize;
            vendorListModel.TotalCount = items.TotalCount;
            vendorListModel.TotalPages = items.TotalPages;
            return vendorListModel;
        }

        private static List<VendorModel> CreateVendorModelList(IPagedList<Vendor> items)
        {
            var vendors = new List<VendorModel>();

            foreach (var item in items)
            {

                vendors.Add(new VendorModel
                {

                    Id = item.Id,
                    VendorCode = item.VendorCode,
                    Name = item.Name,
                    TaxID = item.TaxID,
                    CompanyTypeId = item.CompanyTypeId,
                    CompanyTypeName = item.CompanyType.ToString(),
                    TaxTypeId = item.TaxTypeId,
                    TaxTypeName = item.TaxType.ToString(),
                });
            }

            return vendors;
        }

        [Authorize]
        [HttpPost("/createOrUpdateVendor")]
        public VendorModel CreateOrUpdateVendor(VendorUpdateRequest req)
        {
            var vendor = new Vendor();
            if (req.Id > 0)
                vendor = _vendorService.GetVendorById(req.Id);
            vendor.VendorCode = req.VendorCode;

            if (req.Id > 0)
                _vendorService.UpdateVendor(vendor);
            else
                _vendorService.InsertVendor(vendor);
            var vendorModel = _mapper.Map<VendorModel>(vendor);

            return vendorModel;
        }

     

         [Authorize]
        [HttpGet("{id}")]
        public ActionResult<VendorModel> GetVendor([FromRoute] int id)
        {
            var vendorModel = new VendorModel();

            if (id == 0)
                return vendorModel;

            var vendor = _vendorService.GetVendorById(id);

            if (vendor == null)
                return vendorModel;
            vendorModel = _mapper.Map<VendorModel>(vendor);
            var address = _addressService.GetAddressById(vendor.AddressId);
            var addressModel = _mapper.Map<AddressModel>(address);
            vendorModel.Address = addressModel;

            return vendorModel;

        }



    
    }
}
