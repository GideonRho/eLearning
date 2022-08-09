using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NUnit.Framework;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Requests.Courses;
using WebAPI.Models.API.Responses;
using WebAPI.Models.API.Responses.Questions;
using WebAPI.Models.API.Responses.Statistics;
using WebAPI.Models.API.Responses.Texts;

namespace WebAPI.Tests.Misc
{
    public class Client
    {
        
        public static string url = "http://localhost:5000";
        private const string TestData = "../../../../TestData/";

        private HttpClient _httpClient;
        private string _cookie;

        public long peakTime;
        public double trendTime;
        public long totalTime;
        public long counter;
        public long AverageTime => totalTime / counter;
        public readonly Dictionary<string, long> postTimer = new Dictionary<string, long>();
        public readonly Dictionary<string, long> getTimer = new Dictionary<string, long>();

        public Client()
        {
            Authenticate();
            _httpClient = InitHttpClient();
        }
        
        public UserApi Authenticate(Authenticate data) => Authenticate(data.Username, data.Password);
        public void Register(CreateUser data) => Post(data, "/user/register");
        public void Logout() => Post(null, "/user/logout");
        public void LogoutAll() => Post(null, "/user/logoutAll");

        public List<QuestionApi> ImportQuestions(string excelPath, QuestionImportFilepond payload) => 
            Import<List<QuestionApi>>(payload, excelPath, "/admin/question/import");

        public TextApi ImportTexts(string excelPath, TextImportFilepond payload) => 
            Import<TextApi>(payload, excelPath, "/admin/text/import");

        public List<TextQuestionApi> GetTextQuestions(int id)
            => Get<List<TextQuestionApi>>($"/admin/text/{id}/questions");
        
        public CourseApi CreateCourse(CreateCourse data) => Post<CourseApi>(data, "/admin/course");
        public List<CourseApi> GetCourses() => Get<List<CourseApi>>("/course");

        public void SubmitQuestionnaire(int id, SubmitQuestionnaire data) => Post(data, $"/questionnaire/{id}/submit");
        public QuestionnaireApi CurrentQuestionnaire() => Get<QuestionnaireApi>("/questionnaire/current");
        public List<AnswerApi> QuestionnaireAnswers(int id) => Get<List<AnswerApi>>($"/questionnaire/{id}/answers"); 
        public void StartQuestionnaire(int courseId) => Post(null, $"/questionnaire/start/{courseId}");

        public QuestionnaireStatistic StatisticForQuestionnaire(int id) =>
            Get<QuestionnaireStatistic>($"/statistics/questionnaire/{id}");

        public T Import<T>(object payload, string filePath, string endpoint, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            filePath = $"{TestData}{filePath}";
            
            FileStream stream = new FileStream(filePath, FileMode.Open);
            string json = payload != null ? JsonConvert.SerializeObject(payload) : "";
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", Path.GetFileName(filePath));
            content.Add(new StringContent(json, Encoding.UTF8, "application/json"), "filepond");
            var response = _httpClient.PostAsync($"{url}{endpoint}", content).Result;

            var s = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedCode, response.StatusCode, s);
            var result = JsonConvert.DeserializeObject<T>(s);
            
            return result;
        }
        
        public T Get<T>(string endpoint, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = Get(endpoint);
            
            Assert.AreEqual(expectedCode, response.StatusCode, response.Content.ReadAsStringAsync().Result);
            var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public T Post<T>(object payload, string endpoint, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = Post(payload, endpoint);

            Assert.AreEqual(expectedCode, response.StatusCode, response.Content.ReadAsStringAsync().Result);
            var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            
            return result;
        }
        
        public T Patch<T>(object payload, string endpoint, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = Patch(payload, endpoint);

            Assert.AreEqual(expectedCode, response.StatusCode, response.Content.ReadAsStringAsync().Result);
            var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            
            return result;
        }
        
        private UserApi Authenticate(string username = "Admin", string password = "1234")
        {
            HttpClient client = new HttpClient();

            string endpoint = "/user/authenticate";
            object payload = new Authenticate
            {
                Username = username,
                Password = password
            };
            
            string json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync($"{url}{endpoint}", content).Result;
            var result = JsonConvert.DeserializeObject<UserApi>(response.Content.ReadAsStringAsync().Result);
            
            _cookie = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value.First();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.Content.ReadAsStringAsync().Result);

            _httpClient = InitHttpClient();
            return result;
        }

        private HttpClient InitHttpClient()
        {
            if (_cookie == null) return new HttpClient();
            var baseAddress = new Uri(url);
            
            string[] s = _cookie.Split("=");
            string key = s[0];
            string value = s[1].Split(";")[0];
            
            var cookieContainer = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler{CookieContainer = cookieContainer};
            
            cookieContainer.Add(baseAddress, new Cookie(key, value));
            
            return new HttpClient(handler){BaseAddress = baseAddress};
        }
        
        public HttpResponseMessage Post(object payload, string endpoint)
        {

            string json = payload != null ? JsonConvert.SerializeObject(payload) : "";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            Stopwatch sw = Stopwatch.StartNew();
            var response = _httpClient.PostAsync($"{url}{endpoint}", content).Result;
            sw.Stop();

            string key = RemoveNumbers(endpoint);
            long l = 0;
            postTimer.TryGetValue(key, out l);
            l += sw.ElapsedMilliseconds;
            postTimer[key] = l;
            AddTime(sw.ElapsedMilliseconds);
            
            return response;
        }
        
        public HttpResponseMessage Upload(string filePath, string endpoint)
        {
            filePath = $"{TestData}{filePath}";

            FileStream stream = new FileStream(filePath, FileMode.Open);
            
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", Path.GetFileName(filePath));
            var response = _httpClient.PostAsync($"{url}{endpoint}", content).Result;

            return response;
        }

        public HttpResponseMessage Import(object filepond, string filePath, string endpoint)
        {
            filePath = $"{TestData}{filePath}";

            FileStream stream = new FileStream(filePath, FileMode.Open);
            
            string json = filepond != null ? JsonConvert.SerializeObject(filepond) : "";
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", Path.GetFileName(filePath));
            content.Add(new StringContent(json, Encoding.UTF8, "application/json"), "filepond");
            var response = _httpClient.PostAsync($"{url}{endpoint}", content).Result;

            return response;
        }
        
        public HttpResponseMessage Patch(object payload, string endpoint)
        {

            string json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _httpClient.PatchAsync($"{url}{endpoint}", content).Result;

            return response;
        }
        
        public HttpResponseMessage Get(string endpoint)
        {
            
            Stopwatch sw = Stopwatch.StartNew();
            var response = _httpClient.GetAsync($"{url}{endpoint}").Result;
            sw.Stop();

            string key = RemoveNumbers(endpoint);
            long l = 0;
            getTimer.TryGetValue(key, out l);
            l += sw.ElapsedMilliseconds;
            getTimer[key] = l;
            AddTime(sw.ElapsedMilliseconds);

            return response;
        }

        private void AddTime(long time)
        {
            if (time > peakTime) peakTime = time;
            trendTime += time;
            if (trendTime != time) trendTime /= 2;
            totalTime += time;
            counter += 1;
        }
        
        private string RemoveNumbers(string s) =>  Regex.Replace(s, @"[\d-]", string.Empty);

    }
}