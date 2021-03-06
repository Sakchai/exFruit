﻿
using Microsoft.AspNetCore.Components;
using MissionControl.Client.Util;
using MissionControl.Shared.Models;
using MissionControl.Shared.Models.Common;
using MissionControl.Shared.Models.Purchase;
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
    public class PurchaseState
    {
        public const string BackendUrl = "http://localhost:64170";

        private HttpClient _httpClient = new HttpClient();
        public List<PurchaseModel> Rows;
        public PurchaseListModel purchaseListModel = new PurchaseListModel();
        public PurchaseModel Header;
        public List<PurchaseItemModel> Details;
        public List<SelectListItem> Products = new List<SelectListItem>();
        public List<SelectListItem> Vendors = new List<SelectListItem>();
        public List<SelectListItem> PurchaseStatuses;
        public List<SelectListItem> PurchaseProcesses;

        public PurchaseState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SearchPurchaseAsync(PurchaseSearchRequest purchaseSearchRequest, string token)
        {

            var apiRequest = $"{BackendUrl}/searchPurchase";
            Console.WriteLine(apiRequest);
            if (!string.IsNullOrWhiteSpace(token))
            {

                var strPayload = JsonConvert.SerializeObject(purchaseSearchRequest);
                Console.WriteLine($"strPayload={strPayload}");
                var items = await _httpClient.PostJsonAsync<PurchaseModel[]>(apiRequest, strPayload,
                                new AuthenticationHeaderValue("Bearer", token));
                Rows = new List<PurchaseModel>();
                foreach (var item in items)
                {
                    Rows.Add(item);
                    Console.WriteLine(item.Id);
                }
            }
        }

        public async Task SearchPurchaseListAsync(PurchaseSearchRequest purchaseSearchRequest, string token)
        {

            var apiRequest = $"{BackendUrl}/searchPurchaseList";
            Console.WriteLine(apiRequest);
            if (!string.IsNullOrWhiteSpace(token))
            {

                var strPayload = JsonConvert.SerializeObject(purchaseSearchRequest);
                Console.WriteLine($"strPayload={strPayload}");
                var purchaseListModel = await _httpClient.PostJsonAsync<PurchaseListModel>(apiRequest, strPayload,
                                new AuthenticationHeaderValue("Bearer", token));
                var strItems = JsonConvert.SerializeObject(purchaseListModel);
                Console.WriteLine($"strItems={strItems}");
                Rows = purchaseListModel.Purchases != null ? purchaseListModel.Purchases.ToList() : new List<PurchaseModel>();
                //Products = purchaseListModel.Products != null ? purchaseListModel.Products.ToList() : new List<SelectListItem>();
                //Vendors = purchaseListModel.Vendors != null ? purchaseListModel.Vendors.ToList() : new List<SelectListItem>();
                PurchaseStatuses = purchaseListModel.PurchaseStatus != null ? purchaseListModel.PurchaseStatus.ToList() : new List<SelectListItem>();
                PurchaseProcesses = purchaseListModel.PurchaseProcessStatus != null ? purchaseListModel.PurchaseProcessStatus.ToList() : new List<SelectListItem>();
            }
        }
        public async Task UpdatePurchaseItemAsync(PurchaseItemUpdateRequest updateRequest, string token)
        {
            var apiRequest = $"{BackendUrl}/updatePurchaseItem";
            Console.WriteLine(apiRequest);
            await PostPurchaseItemAsync(updateRequest, token, apiRequest);
            await GetPurchaseAsync(updateRequest.PurchaseId, token);
        }

        public async Task AddPurchaseItemAsync(PurchaseItemUpdateRequest updateRequest, string token)
        {
            var apiRequest = $"{BackendUrl}/addPurchaseItem";
            Console.WriteLine(apiRequest);
            await PostPurchaseItemAsync(updateRequest, token, apiRequest);
            await GetPurchaseAsync(updateRequest.PurchaseId, token);
        }

        private async Task PostPurchaseItemAsync(PurchaseItemUpdateRequest updateRequest, string token, string apiRequest)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var strPayload = JsonConvert.SerializeObject(updateRequest);
                Console.WriteLine($"strPayload={strPayload}");
                var items = await _httpClient.PostJsonAsync<PurchaseItemModel[]>(apiRequest, strPayload,
                                new AuthenticationHeaderValue("Bearer", token));
                Details = new List<PurchaseItemModel>();
                foreach (var item in items)
                    Details.Add(item);
            }
        }


        public async Task AddPurchaseAsync(PurchaseModel purchase, string token)
        {
            var apiRequest = $"{BackendUrl}/createOrUpdatePurchase";
            Console.WriteLine(apiRequest);
            var updateRequest = new PurchaseUpdateRequest
            {
                Id = purchase.Id,
                PurchaseDate = purchase.PurchaseDate,
                PurchaseNo = purchase.PurchaseNo,
                PurchaseStatusIdValue = purchase.PurchaseStatusIdValue,
                PurchaseProcessIdValue = "10", //Purchase Status
                Remark = purchase.Remark,
                VendorName = purchase.VendorName,
                VendorAddress = purchase.VendorAddress,
                WeightKg = purchase.WeightKg,
                TotalCrates = purchase.TotalCrates,
            };
            await PostPurchaseAsync(updateRequest, token, apiRequest);
        }

        private async Task PostPurchaseAsync(PurchaseUpdateRequest updateRequest, string token, string apiRequest)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var strPayload = JsonConvert.SerializeObject(updateRequest);
                Console.WriteLine($"strPayload={strPayload}");
                Header = await _httpClient.PostJsonAsync<PurchaseModel>(apiRequest, strPayload,
                                new AuthenticationHeaderValue("Bearer", token));
            }
        }
        public async Task DeletePurchaseItemAsync(int id, string token)
        {
            var apiRequest = $"{BackendUrl}/deletePurchaseItem/{id}";

            await GetPurchaseItemList(id, token, apiRequest);
            await GetPurchaseAsync(Details.FirstOrDefault().PurchaseId, token);

        }

        private async Task GetPurchaseItemList(int id, string token, string apiRequest)
        {
            Details = new List<PurchaseItemModel>();
            if (!string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine($"id={id}");
                var items = await _httpClient.GetJsonAsync<PurchaseItemModel[]>(apiRequest,
                                new AuthenticationHeaderValue("Bearer", token));
                foreach (var item in items)
                {
                    Details.Add(item);
                }
            }
        }

        public async Task GetPurchaseAsync(int id, string token)
        {
            var apiRequest = $"{BackendUrl}/purchases/{id}";
            Console.WriteLine(apiRequest);
            Header = new PurchaseModel();
            if (!string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine($"id={id}");
                Header = await _httpClient.GetJsonAsync<PurchaseModel>(apiRequest,
                                new AuthenticationHeaderValue("Bearer", token));
                var strPayload = JsonConvert.SerializeObject(Header);
                Console.WriteLine($"strPayload={strPayload}");
                //Products = Header.Products != null ? Header.Products.ToList() : new List<SelectListItem>();
                //Vendors = Header.Vendors != null ? Header.Vendors.ToList() : new List<SelectListItem>();

            }
            Console.WriteLine($"purchase Id:{Header.Id}");
            Console.WriteLine($"purchase PurchaseNo:{Header.PurchaseNo}");

        }

        public async Task GetPurchaseItemsAsync(int id, string token)
        {
            var apiRequest = $"{BackendUrl}/purchaseItems/{id}";
            await GetPurchaseItemList(id, token, apiRequest);
        }

    }
}
