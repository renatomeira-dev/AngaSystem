using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net;
using System.Collections.Concurrent;

namespace AngaSystem.API.Helpers
{
    public class ConfigClient
    {
        public string UrlBase { get; set; }
        public string Method { get; set; }
        public bool Compress { get; set; }
        public bool CustomHandlerClient { get; set; }
        public TimeSpan TimeOut { get; set; }
        public string Authentication { get; set; }
        public string Credential { get; set; }
        public string AcceptEncoding { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ConfigClient)) { return false; }

            var other = (ConfigClient)obj;

            return this.UrlBase.Equals(other.UrlBase) &&
                   this.Method.Equals(other.Method) &&
                   this.Compress.Equals(other.Compress) &&
                   this.CustomHandlerClient.Equals(other.CustomHandlerClient) &&
                   this.TimeOut.Equals(other.TimeOut) &&
                   (this.Authentication ?? "*").Equals(other.Authentication ?? "*") &&
                   (this.Credential ?? "*").Equals(other.Credential ?? "*") &&
                   (this.AcceptEncoding ?? "*").Equals(other.AcceptEncoding ?? "*");
        }

        public override int GetHashCode()
        {
            return String.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                                this.UrlBase,
                                this.Method,
                                this.Compress,
                                this.CustomHandlerClient,
                                this.TimeOut,
                                this.Authentication ?? "*",
                                this.Credential ?? "*",
                                this.AcceptEncoding ?? "*").GetHashCode();
        }
    }
    public class WebApiClient
    {
        private TimeSpan timeoutDefault = new TimeSpan(1, 0, 0);
        private static ConcurrentDictionary<ConfigClient, HttpClient> clients = new ConcurrentDictionary<ConfigClient, HttpClient>();

        private HttpClient GetClient(string urlBase, string method, bool compress, bool customHandlerClient, TimeSpan timeOut, string authentication, string credential, string acceptEncoding)
        {
            var config = new ConfigClient()
            {
                UrlBase = urlBase,
                Method = method,
                Compress = compress,
                CustomHandlerClient = customHandlerClient,
                TimeOut = timeOut,
                AcceptEncoding = acceptEncoding,
                Authentication = authentication,
                Credential = credential
            };

            try
            {
                HttpClient client = null;
                if (clients.ContainsKey(config))
                {
                    client = clients[config];
                }

                if (client != null)
                    return client;
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            HttpClient clientNew = null;

            if (!compress && !customHandlerClient)
            {
                clientNew = new HttpClient();
                clientNew.DefaultRequestHeaders.Clear();
            }

            else
            {
                clientNew = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip });

                clientNew.DefaultRequestHeaders.Clear();

                if (acceptEncoding == "gzip")
                    clientNew.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                else
                    clientNew.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                clientNew.DefaultRequestHeaders.Connection.Clear();
                clientNew.DefaultRequestHeaders.Connection.Add("Keep-Alive");
                clientNew.DefaultRequestHeaders.ExpectContinue = false;
            }

            clientNew.DefaultRequestHeaders.ConnectionClose = false;
            clientNew.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientNew.Timeout = timeOut;

            if (!String.IsNullOrEmpty(authentication) && !String.IsNullOrEmpty(credential))
            {
                clientNew.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authentication, credential);
            }

            var i = clients.TryAdd(config, clientNew);

            return clientNew;
        }

        public HttpResponseMessage GetAsync(string urlBase, string action, string authentication = "", string credential = "")
        {
            try
            {
                var client = GetClient(urlBase, "GET", false, false, timeoutDefault, authentication, credential, "");
                var uri = urlBase;

                if (urlBase != action)
                {
                    if (action.Contains(urlBase))
                        uri = action;
                    else
                        uri += "/" + action;
                }

                var response = client.GetAsync(uri).Result;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HttpResponseMessage PostAsync<TSend>(string urlBase, string action, TSend sendObject, string authentication = "", string credential = "")
        {
            try
            {
                var client = GetClient(urlBase, "POST", false, false, timeoutDefault, authentication, credential, "");
                var uri = urlBase;

                if (urlBase != action)
                {
                    if (action.Contains(urlBase))
                        uri = action;
                    else
                        uri += "/" + action;
                }

                var mediaTypeFormatter = new JsonMediaTypeFormatter
                {
                    Indent = true
                };

                var content = (new ObjectContent<TSend>(sendObject, mediaTypeFormatter));

                var response = client.PostAsJsonAsync(uri, sendObject).Result;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HttpResponseMessage PutAsync<TSend>(string urlBase, string action, TSend sendObject, string authentication = "", string credential = "")
        {
            var client = GetClient(urlBase, "PUT", false, false, timeoutDefault, authentication, credential, "");
            var uri = urlBase;

            if (urlBase != action)
            {
                if (action.Contains(urlBase))
                    uri = action;
                else
                    uri += "/" + action;
            }

            var content = (new ObjectContent<TSend>(sendObject, new JsonMediaTypeFormatter()));

            try
            {
                var jsonString = content.ReadAsStringAsync().Result;
            }
            catch
            { }

            var response = client.PutAsJsonAsync(uri, sendObject).Result;
            return response;
        }
    }
}
