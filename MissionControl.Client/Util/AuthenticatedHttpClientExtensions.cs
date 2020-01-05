using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MissionControl.Client.Util
{
    public static class AuthenticatedHttpClientExtensions
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = authorization;
            Console.WriteLine(url);
            var response = await httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static async Task<T> PostJsonAsync<T>(this HttpClient httpClient, string url, string jsonParms, AuthenticationHeaderValue authorization)
        {
            httpClient.DefaultRequestHeaders.Authorization = authorization;
            var content = new StringContent(jsonParms, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(url),
                Content = content
            };

            requestMessage.Headers.Authorization = authorization;

            var response = await httpClient.SendAsync(requestMessage);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


    }

    public class PostResponse
    {

        public string StatusCode { get; set; }
        public string BodyContent { get; set; }
    }
}
