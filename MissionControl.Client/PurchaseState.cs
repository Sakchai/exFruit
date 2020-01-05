
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

        public async Task DeletePurchaseItemAsync(int id, string token)
        {
            var apiRequest = $"{BackendUrl}/deletePurchaseItem/{id}";

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
            Details = new List<PurchaseItemModel>();
            if (!string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine($"id={id}");
                var items = await _httpClient.GetJsonAsync<PurchaseItemModel[]>(apiRequest,
                                new AuthenticationHeaderValue("Bearer", token));
               // Console.WriteLine($"items={JsonConvert.SerializeObject(items)}");
                foreach (var item in items)
                {
                  //  Console.WriteLine($"item={JsonConvert.SerializeObject(item)}");
                    Details.Add(item);
                }
                Console.WriteLine($"purchaseItems={JsonConvert.SerializeObject(Details)}");
            }
        }

        public void BarcodeItem(int id, string token)
        {
            var apiRequest = $"{BackendUrl}/editPurchase";
            if (id != 0)
            {

            }
        }
    }
}
