using AutoMapper;
using MissionControl.Shared;
using MissionControl.Shared.Models;
using MissionControl.Shared.Models.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MissionControl.Server
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //CreateMap<Purchase, PurchaseModel>()
            //    .ForMember(x => x.PurchaseTotalValue, opt => opt.Ignore())
            //    .ForMember(x => x.PurchaseStatusName, opt => opt.Ignore())
            //    .ForMember(x => x.PurchaseProcessName, opt => opt.Ignore())
            //    .ForMember(x => x.PurchaseDateName, opt => opt.Ignore())
            //    .ForMember(x => x.Products, opt => opt.Ignore())
            //    .ForMember(x => x.Vendors, opt => opt.Ignore());
            //// .ForMember(x => x.BillingAddress, opt => opt.Ignore());
            //CreateMap<PurchaseItem, ReceptionItemModel>(); // means you want to map from User to UserDTO
            //CreateMap<Reception, ReceptionModel>();
            CreateMap<Address, AddressModel>();
            CreateMap<Vendor, VendorModel>()
                 .ForMember(x => x.Address, opt => opt.Ignore())
                 .ForMember(x => x.CompanyTypeName, opt => opt.Ignore())
                 .ForMember(x => x.TaxTypeName, opt => opt.Ignore())
                 .ForMember(x => x.CountryTypeName, opt => opt.Ignore());

        }
    }
}
