
using Microsoft.AspNetCore.Components;
using MissionControl.Client.Util;
using MissionControl.Shared.Models;
using MissionControl.Shared.Models.Common;
using MissionControl.Shared.Models.Vendor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MissionControl.Client
{
    public class VendorState
    {

        private HttpClient _httpClient = new HttpClient();
        public List<VendorModel> Rows;
        public VendorListModel vendorListModel = new VendorListModel();
        public VendorModel Vendor = new VendorModel();
        public AddressModel Address = new AddressModel();
        public List<SelectListItem> CountryTypes;
        public List<SelectListItem> TaxTypes;

        public VendorState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SearchVendorListAsync(VendorSearchRequest vendorSearchRequest, string token)
        {

            var apiRequest = $"/searchVendorList";
            Console.WriteLine(apiRequest);
            if (!string.IsNullOrWhiteSpace(token))
            {

                var strPayload = JsonConvert.SerializeObject(vendorSearchRequest);
                Console.WriteLine($"strPayload={strPayload}");
                var vendorListModel = await _httpClient.PostJsonAsync<VendorListModel>(apiRequest, strPayload,
                                new AuthenticationHeaderValue("Bearer", token));
                var strItems = JsonConvert.SerializeObject(vendorListModel);
                Console.WriteLine($"strItems={strItems}");
                Rows = vendorListModel.Vendors != null ? vendorListModel.Vendors.ToList() : new List<VendorModel>();
                //Products = vendorListModel.Products != null ? vendorListModel.Products.ToList() : new List<SelectListItem>();
                //Vendors = vendorListModel.Vendors != null ? vendorListModel.Vendors.ToList() : new List<SelectListItem>();
                CountryTypes = vendorListModel.CompanyTypes != null ? vendorListModel.CompanyTypes.ToList() : new List<SelectListItem>();
                TaxTypes = vendorListModel.TaxTypes != null ? vendorListModel.TaxTypes.ToList() : new List<SelectListItem>();
            }
        }
   
        public async Task AddVendorAsync(VendorModel vendor, string token)
        {
            var apiRequest = $"/createOrUpdateVendor";
            Console.WriteLine(apiRequest);
            var updateRequest = new VendorUpdateRequest
            {
                Id = vendor.Id,
            };
            await PostVendorAsync(updateRequest, token, apiRequest);
        }

        private async Task PostVendorAsync(VendorUpdateRequest updateRequest, string token, string apiRequest)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var strPayload = JsonConvert.SerializeObject(updateRequest);
                Console.WriteLine($"strPayload={strPayload}");
                Vendor = await _httpClient.PostJsonAsync<VendorModel>(apiRequest, strPayload,
                                new AuthenticationHeaderValue("Bearer", token));
            }
        }




        public async Task GetVendorAsync(int id, string token)
        {
            var apiRequest = $"/vendors/{id}";
            Console.WriteLine(apiRequest);
            if (!string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine($"id={id}");
                Vendor = await _httpClient.GetJsonAsync<VendorModel>(apiRequest,
                                new AuthenticationHeaderValue("Bearer", token));
                var strPayload = JsonConvert.SerializeObject(Vendor);
                Console.WriteLine($"strPayload={strPayload}");
                //Products = Header.Products != null ? Header.Products.ToList() : new List<SelectListItem>();
                //Vendors = Header.Vendors != null ? Header.Vendors.ToList() : new List<SelectListItem>();
                Console.WriteLine($"vendor Id:{Vendor.Id}");
                if (Vendor.Address != null)
                    Console.WriteLine($"vendor VendorNo:{Vendor.Address.District}");

            }

        }

        public async Task DeleteVendorAsync(int id, string token)
        {
            var apiRequest = $"/deletePurchase/{id}";



        }

    }
}
