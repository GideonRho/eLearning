using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using WebAPI.Models.API.Requests;

namespace WebAPI.Tests
{
    public static class Handler
    {

        private const string Url = "http://localhost:5000";
        private const string TestData = "../../../../TestData/";
        private static string _cookie;

        private static Stopwatch _timeout;

        public enum EServerMode
        {
            Empty, WithStaticData, WithTestData, Restart
        }
        
        public static void StartServer(EServerMode mode = EServerMode.WithTestData)
        {

            switch (mode)
            {
                case EServerMode.Empty:
                    DeployTestDatabase("deployEmpty");
                    break;
                case EServerMode.WithStaticData:
                    DeployTestDatabase("deployWithStatic");
                    break;
                case EServerMode.WithTestData:
                    DeployTestDatabase("deployWithTestData");
                    break;
                case EServerMode.Restart:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
            
            _timeout = Stopwatch.StartNew();
            while (!PortIsOpen() && CheckTimeout()) Thread.Sleep(1);
            var task = new Task(() => Program.Main(new string[0]));
            task.Start();
            _timeout = Stopwatch.StartNew();
            while (!TryConnectServer() && CheckTimeout()) Thread.Sleep(1);
            
            Authenticate();
        }

        private static bool CheckTimeout(int time = 5000)
        {
            if (_timeout.ElapsedMilliseconds > time) throw new TimeoutException();
            return true;
        }

        private static bool PortIsOpen()
        {
            Thread.Sleep(1000);
            return true;
        }
        
        private static bool TryConnectServer()
        {

            try
            {
                var response = Get($"/course/active");
                return response.StatusCode > 0;
            }
            catch
            {
                return false;
            }

            
        }

        private static void Authenticate()
        {
            HttpClient client = new HttpClient();

            string endpoint = "/user/authenticate";
            object payload = new Authenticate
            {
                Username = "Admin",
                Password = "1234"
            };
            
            string json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync($"{Handler.Url}{endpoint}", content).Result;
            
            _cookie = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value?.First();
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.Content.ReadAsStringAsync().Result);
            Assert.NotNull(_cookie);
            Assert.False(string.IsNullOrEmpty(_cookie));

        }

        private static HttpClient InitHttpClient()
        {
            if (_cookie == null) return new HttpClient();
            var baseAddress = new Uri(Url);
            
            string[] s = _cookie.Split("=");
            string key = s[0];
            string value = s[1].Split(";")[0];
            
            var cookieContainer = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler{CookieContainer = cookieContainer};
            
            cookieContainer.Add(baseAddress, new Cookie(key, value));
            
            return new HttpClient(handler){BaseAddress = baseAddress};
        }
        
        public static HttpResponseMessage Post(object payload, string endpoint)
        {
            HttpClient client = InitHttpClient();

            string json = payload != null ? JsonConvert.SerializeObject(payload) : "";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync($"{Handler.Url}{endpoint}", content).Result;

            return response;
        }
        
        public static HttpResponseMessage Upload(string filePath, string endpoint)
        {
            filePath = $"{TestData}{filePath}";
            HttpClient client = InitHttpClient();
            
            FileStream stream = new FileStream(filePath, FileMode.Open);
            
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", Path.GetFileName(filePath));
            var response = client.PostAsync($"{Handler.Url}{endpoint}", content).Result;

            return response;
        }

        public static HttpResponseMessage Import(object filepond, string filePath, string endpoint)
        {
            filePath = $"{TestData}{filePath}";
            HttpClient client = InitHttpClient();
            
            FileStream stream = new FileStream(filePath, FileMode.Open);
            
            string json = filepond != null ? JsonConvert.SerializeObject(filepond) : "";
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", Path.GetFileName(filePath));
            content.Add(new StringContent(json, Encoding.UTF8, "application/json"), "filepond");
            var response = client.PostAsync($"{Handler.Url}{endpoint}", content).Result;

            return response;
        }
        
        public static HttpResponseMessage Patch(object payload, string endpoint)
        {
            HttpClient client = InitHttpClient();

            string json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PatchAsync($"{Handler.Url}{endpoint}", content).Result;

            return response;
        }
        
        public static HttpResponseMessage Get(string endpoint)
        {
            HttpClient client = InitHttpClient();

            var response = client.GetAsync($"{Handler.Url}{endpoint}").Result;

            return response;
        }
        
        private static void DeployTestDatabase(string script)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "/bin/bash";
            psi.Arguments = $"/home/senju/Workspace/GitLab/elearningbackend/Scripts/Development/{script}.sh";
            Process p = Process.Start(psi);
            p.WaitForExit();
        }

    }
}