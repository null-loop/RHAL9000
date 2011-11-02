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

        private static readonly ILog Logger = LogManager.GetLogger(typeof (HttpXmlAccessor));

        public HttpXmlAccessor(Uri baseUrl, string urlPrefix, string username, string password)
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

        public Uri BaseUrl { get; private set; }

        public XElement GetXml(string path)
        {
            var requestUri = GetRequestUri(path);
            var request = CreateWebRequest(requestUri);

            try
            {
                Logger.DebugFormat("Sending request for {0}", requestUri);

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response == null)
                        throw new InvalidOperationException("Received null or non HttpWebResponse");

                    Logger.DebugFormat("Received response code {0}", response.StatusCode);

                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream == null)
                            throw new InvalidOperationException("Received null response stream");

                        using (var reader = new StreamReader(responseStream))
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
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                if (response != null)
                {
                    Logger.DebugFormat("Received response code {0}", response.StatusCode);

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                }

                throw CreateXmlAccessException(requestUri, ex);
            }
            catch (Exception ex)
            {
                throw CreateXmlAccessException(requestUri, ex);
            }
        }

        private static Exception CreateXmlAccessException(Uri requestUri, Exception ex)
        {
            Logger.Error("Error occurred getting " + requestUri, ex);
            return new XmlAccessException("Error occurred attempting to access URI : " + requestUri, ex);
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
                return new Uri(BaseUrl, UrlPrefix + path);
            }
            if (string.IsNullOrEmpty(UrlPrefix) && !string.IsNullOrEmpty(path))
            {
                return new Uri(BaseUrl, path);
            }
            if (string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(UrlPrefix))
            {
                return new Uri(BaseUrl, UrlPrefix);
            }
            if (string.IsNullOrEmpty(UrlPrefix) && string.IsNullOrEmpty(path))
            {
                return BaseUrl;
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