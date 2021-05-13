using System;
using System.IO;
using System.Net;


namespace PlantWellBgClient.Services
{
    class CoreProxy
    {
        private static Uri uri;
        private static string requestType;
        private string data;
        private string host = "localhost";
        public CoreProxy(string UriEndpoint, string rType, string data) {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = host;
            uriBuilder.Path = UriEndpoint;
            uri = uriBuilder.Uri;
            requestType = rType;
            Request(data);
        }  
        public static string Request(string data)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);
            var result = "";
            HttpWebResponse httpResponse = null;
            httpRequest.Accept = "application/json";
            if (requestType == "get")
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            else if (requestType == "post")
            {
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = data;
                httpRequest.Headers["Content-Length"] = "0";
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}
  