using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Resolvers;
using log4net;

namespace RHAL9000.Core
{
    public class HttpXmlAccessor : IXmlAccessor
    {
        public static XNamespace Xhtml = XNamespace.Get("http://www.w3.org/1999/xhtml");

        private static readonly ILog Log = LogManager.GetLogger(typeof (HttpXmlAccessor));

        public HttpXmlAccessor(string baseUrl, string urlPrefix, string username, string password)
        {
            BaseUrl = baseUrl;
            UrlPrefix = urlPrefix;
            Username = username;
            Password = password;
        }

        private string UrlPrefix { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }

        #region IXmlAccessor Members

        public string BaseUrl { get; private set; }

        public XElement GetXml(string path)
        {
            var requestUri = GetRequestUri(path);
            var request = CreateWebRequest(requestUri);

            try
            {
                Log.DebugFormat("Sending request for {0}", requestUri);

                using (WebResponse response = request.GetResponse())
                {
                    var httpResponse = (HttpWebResponse) response;

                    Log.DebugFormat("Received response code {0}", httpResponse.StatusCode);

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var settings = new XmlReaderSettings
                                           {
                                               DtdProcessing = DtdProcessing.Parse,
                                               XmlResolver = new XmlPreloadedResolver(XmlKnownDtds.Xhtml10)
                                           };
                        return XDocument.Load(XmlReader.Create(reader, settings)).Root;
                    }
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                if (response != null && ex.Status == WebExceptionStatus.ProtocolError &&
                    response.StatusCode == HttpStatusCode.NotFound)
                {
                    Log.DebugFormat("Received response code {0}", response.StatusCode);

                    return null;
                }

                Log.Error("Error occurred getting " + requestUri, ex);
                throw new XmlAccessException("Error occurred attempting to access URI : " + requestUri, ex);
            }
        }

        public XElement GetXml()
        {
            return GetXml(string.Empty);
        }

        #endregion

        private Uri GetRequestUri(string path)
        {
            if (!string.IsNullOrEmpty(UrlPrefix) && !string.IsNullOrEmpty(path))
            {
                return new Uri(new Uri(BaseUrl, UriKind.Absolute), UrlPrefix + path);
            }
            if (string.IsNullOrEmpty(UrlPrefix) && !string.IsNullOrEmpty(path))
            {
                return new Uri(new Uri(BaseUrl, UriKind.Absolute), path);
            }
            if (string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(UrlPrefix))
            {
                return new Uri(new Uri(BaseUrl, UriKind.Absolute), UrlPrefix);
            }
            if (string.IsNullOrEmpty(UrlPrefix) && string.IsNullOrEmpty(path))
            {
                return new Uri(BaseUrl, UriKind.Absolute);
            }
            throw new InvalidOperationException("Cannot formulate request URI");
        }

        private HttpWebRequest CreateWebRequest(Uri requestUri)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUri);

            if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Password))
            {
                httpWebRequest.Credentials = new NetworkCredential(Username, Password);
                httpWebRequest.Method = WebRequestMethods.Http.Get;
            }

            return httpWebRequest;
        }
    }
}