using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace TasksManager.WpfClient.Data
{
    class HttpRequester
    {
        private string baseUrl;
        private HttpClient client;

        public HttpRequester(string baseUrl)
        {
            this.baseUrl = baseUrl;
            this.client = new HttpClient();
        }

        public T Get<T>(string serviceUrl, IDictionary<string, string> headers = null, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Get;

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = client.SendAsync(request).Result;
            CheckResponse(response);
            var returnObj = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(returnObj);
        }

        public T Post<T>(string serviceUrl, object data, 
            IDictionary<string,string> headers = null, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Post;

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            
            var response = client.SendAsync(request).Result;

            CheckResponse(response);

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }
        
        public bool Put(string serviceUrl,
            IDictionary<string, string> headers = null, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Post;

            //erequest.Content = new StringContent(JsonConvert.SerializeObject(data));
            //request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);            

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            try
            {
                var response = client.SendAsync(request).Result;
                CheckResponse(response);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<T> CreateGetRequestAsync<T>(string serviceUrl, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Get;

            return client.SendAsync(request).ContinueWith(
                (task) =>
                {
                    var response = task.Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<T>(content);
                    return result;
                });
        }

        public Task<T> PostAsync<T>(string serviceUrl,
            object data, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Post;

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return client.SendAsync(request).ContinueWith((task) =>
            {
                var response = task.Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(content);
                return result;
            });
        }

        public Task PostAsync(string serviceUrl,
            object data, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Post;

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return client.SendAsync(request);
        }

        private static void CheckResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadGateway
                || response.StatusCode == HttpStatusCode.Unauthorized
                || response.StatusCode >= HttpStatusCode.BadRequest)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;

                if (errorContent.IndexOf("Message") < 0)
                {
                    throw new HttpRequestException(response.StatusCode.ToString());
                }

                var responseObject = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                throw new HttpRequestException(responseObject.Message);
            }
        }

        internal class ErrorResponse
        {
            public string Message { get; set; }
        }
    }
}
