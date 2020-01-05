
using Microsoft.AspNetCore.Components;
using MissionControl.Client.Util;
using MissionControl.Shared.Models;
using MissionControl.Shared.Models.Purchase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public PurchaseModel Header;
        public List<PurchaseItemModel> Details;

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

        public async Task UpdatePurchaseItemAsync(PurchaseItemUpdateRequest updateRequest, string token)
        {
            var apiRequest = $"{BackendUrl}/updatePurchaseItem";
            Console.WriteLine(apiRequest);
            await PostPurchaseItem(updateRequest, token, apiRequest);
        }

        public async Task AddPurchaseItemAsync(PurchaseItemUpdateRequest updateRequest, string token)
        {
            var apiRequest = $"{BackendUrl}/addPurchaseItem";
            Console.WriteLine(apiRequest);
            await PostPurchaseItem(updateRequest, token, apiRequest);
        }

        private async Task PostPurchaseItem(PurchaseItemUpdateRequest updateRequest, string token, string apiRequest)
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

        public async Task DeletePurchaseItemAsync(int id, string token)
        {
            var apiRequest = $"{BackendUrl}/deletePurchaseItem/{id}";

            await GetPurchaseItemList(id, token, apiRequest);

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
            }
            Console.WriteLine($"purchase Id:{Header.Id}");
            Console.WriteLine($"purchase PurchaseNo:{Header.PurchaseNo}");
        }

        public async Task GetPurchaseItemsAsync(int id, string token)
        {
            var apiRequest = $"{BackendUrl}/purchaseItems/{id}";
            await GetPurchaseItemList(id, token, apiRequest);
        }

        public void BarcodeItem(int id, string token)
        {
            var apiRequest = $"{BackendUrl}/barcodePurchase";
            if (id != 0)
            {

            }
        }
    }
}
