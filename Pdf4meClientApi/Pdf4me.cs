﻿//using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pdf4meClient
{
    public class Pdf4me
    {
        HttpClient httpClient;
        public static readonly Pdf4me Instance = new Pdf4me();

        string _api = "https://api.pdf4me.com";
        //string _authString = "https://login.microsoftonline.com/devynooxlive.onmicrosoft.com";
        //string _clientId = "";
        //string _key = "";
        string _basicToken = "";

        static Pdf4me()
        {


        }

        // Instance constructor is private to enforce singleton semantics
        private Pdf4me() : base()
        {
            //_clientId = ConfigurationManager.AppSettings["Pdf4meClientId"];
            //_key = ConfigurationManager.AppSettings["Pdf4meSecret"];
        }

        ///// <summary>
        ///// Initialise with Client Id and Key
        ///// </summary>
        ///// <param name="clientId">ClientId</param>
        ///// <param name="key">Secret</param>
        ///// <param name="api">Api url to be set. If passed null, url will https://api-dev.pdf4me.com</param>
        //public void Init(string clientId, string key, string api)
        //{
        //    _clientId = clientId;
        //    _key = key;

        //    if (!string.IsNullOrEmpty(api))
        //    {
        //        _api = api;
        //    }

        //    _basicToken = "";
        //}

        /// <summary>
        /// Initialise with basic token
        /// </summary>
        /// <param name="basicToken">Token</param>
        /// <param name="api">Api url to be set. If passed null, url will https://api-dev.pdf4me.com</param>
        public void Init(string basicToken, string api = "")
        {
            _basicToken = basicToken;

            if (!string.IsNullOrEmpty(api))
            {
                _api = api;
            }
        }

        public void Init(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public ConvertClient ConvertClient
        {
            get
            {
                return new ConvertClient(getApi());
            }
        }

        public ExtractClient ExtractClient
        {
            get
            {
                return new ExtractClient(getApi());
            }
        }

        public ImageClient ImageClient
        {
            get
            {
                return new ImageClient(getApi());
            }
        }

        public DocumentClient DocumentClient
        {
            get
            {
                return new DocumentClient(getApi());
            }
        }

        public JobClient JobClient
        {
            get
            {
                return new JobClient(getApi());
            }
        } 

        public ManagementClient ManagementClient
        {
            get
            {
                return new ManagementClient(getApi());
            }
        }

        public MergeClient MergeClient
        {
            get
            {
                return new MergeClient(getApi());
            }
        }

        public OcrClient OcrClient
        {
            get
            {
                return new OcrClient(getApi());
            }
        }

        public OptimizeClient OptimizeClient
        {
            get
            {
                return new OptimizeClient(getApi());
            }
        }

        public PdfAClient PdfAClient
        {
            get
            {
                return new PdfAClient(getApi());
            }
        }

        public SplitClient SplitClient
        {
            get
            {
                return new SplitClient(getApi());
            }
        }

        public StampClient StampClient
        {
            get
            {
                return new StampClient(getApi());
            }
        }

        public BarcodeClient BarcodeClient
        {
            get
            {
                return new BarcodeClient(getApi());
            }
        }

        //public UserClient UserClient
        //{
        //    get
        //    {
        //        return new UserClient(getApi());
        //    }
        //}

        public SwissQRClient SwissQRClient
        {
            get
            {
                return new SwissQRClient(getApi());
            }
        }

        public ConvertFromPdfClient ConvertFromPdfClient
        {
            get
            {
                return new ConvertFromPdfClient(getApi());
            }
        }

        public WorkFlowClient WorkFlowClient
        {
            get
            {
                return new WorkFlowClient(getApi());
            }
        }

        public HttpClient getApi()
        {
            
            if (!string.IsNullOrEmpty(_basicToken) && httpClient == null)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 10, 0);


                //var byteArray = Encoding.ASCII.GetBytes($"{_clientId}:{_key}");
                //var byteArray = Encoding.ASCII.GetBytes($"{_basicToken}");
                //var basicToken = Convert.ToBase64String(byteArray);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _basicToken);

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.UserAgent.Clear();
                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("pdf4me-dotnet", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));

                Uri apiUri = new Uri(_api);
                httpClient.BaseAddress = apiUri;
            }
            else if (httpClient == null) 
                throw new ApplicationException("Missing token for authentication, please give BasicToken");

            return httpClient;
        }
    }
}
