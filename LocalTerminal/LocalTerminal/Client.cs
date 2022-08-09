using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using ModelLibrary.Models.API.Requests;
using Newtonsoft.Json;

namespace LocalTerminal
{
    public class Client
    {

        private const string Url = "http://localhost:5000";
        
        private readonly HttpClient _httpClient;

        public Client()
        {
            _httpClient = new HttpClient();
        }

        public List<string> GenerateKeys(GenerateKeysPayload payload)
            => Post<List<string>>(payload, "/local/product/generate");

        public T Post<T>(object payload, string endpoint)
        {
            var response = Post(payload, endpoint);
            
            var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            
            return result;
        }
        
        public HttpResponseMessage Post(object payload, string endpoint)
        {

            string json = payload != null ? JsonConvert.SerializeObject(payload) : "";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = _httpClient.PostAsync($"{Url}{endpoint}", content).Result;

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);
            
            return response;
        }
        
    }
}