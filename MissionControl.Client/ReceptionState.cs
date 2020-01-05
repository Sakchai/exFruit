
using Microsoft.AspNetCore.Components;
using MissionControl.Client.Util;
using MissionControl.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace MissionControl.Client
{
    public class ReceptionState
    {

        private HttpClient _httpClient = new HttpClient();
        //private readonly List<PurchaseItem> _rows = new List<PurchaseItem>();
        public List<ReceptionItemModel> Rows = new List<ReceptionItemModel>();
        public ReceptionState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task AddItemAsync(string ean, string token)
        {
            Console.WriteLine(token);
            var apiRequest = string.Format("/receptionItem/{0}", ean);
            Console.WriteLine(apiRequest);
            if (!string.IsNullOrWhiteSpace(token))
            {
                //var item = await _httpClient.GetJsonAsync<ReceptionItemModel>(apiRequest);
                var item = await _httpClient.GetJsonAsync<ReceptionItemModel>(apiRequest,
                                new AuthenticationHeaderValue("Bearer", token));
                Console.WriteLine(item.ProductName);
                if (item.Id != 0)
                    Rows.Add(item);
            }
        }

        public async Task SaveItemsAsync(string user,string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                //string apiRequest = "/receptions";
                var receptionModel = new ReceptionModel {ReceivedBy = user };
                foreach (var row in Rows) {
                    receptionModel.ReceptionItems.Add(row);
                }
                string json = JsonConvert.SerializeObject(receptionModel);
                //var postResponse = await _httpClient.PostJsonAsync(apiRequest, json, new AuthenticationHeaderValue("Bearer", token));
                //Console.WriteLine(postResponse.StatusCode);
                //Console.WriteLine(postResponse.BodyContent);
                //if (postResponse.StatusCode.Equals("200"))
                //{
                //    Rows = new List<ReceptionItemModel>();
                //}
            }
        }

        public void RemoveItem(int id)
        {
            if (id != 0)
            {
                var existingRow = Rows.SingleOrDefault(r => r.Id == id);
                if (existingRow != null)
                {
                    Rows.Remove(existingRow);
                }
            }
        }
    }
}
