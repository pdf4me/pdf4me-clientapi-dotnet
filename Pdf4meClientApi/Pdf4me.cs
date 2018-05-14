﻿using Microsoft.IdentityModel.Clients.ActiveDirectory;
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

        public static readonly Pdf4me Instance = new Pdf4me();

        string _api = "https://api-dev.pdf4me.com";
        string _authString = "https://login.microsoftonline.com/devynooxlive.onmicrosoft.com";
        string _clientId = "";
        string _key = "";
        string _token = "";

        static Pdf4me()
        {


        }

        // Instance constructor is private to enforce singleton semantics
        private Pdf4me() : base()
        {
            //_clientId = ConfigurationManager.AppSettings["Pdf4meClientId"];
            //_key = ConfigurationManager.AppSettings["Pdf4meSecret"];
        }

        public void Init(string clientId, string key, string api = null)
        {
            _clientId = clientId;
            _key = key;

            if (!string.IsNullOrEmpty(api))
            {
                _api = api;
            }
        }

        public void Init(string token)
        {
            _token = token;
        }

        public DocumentClient DocumentClient
        {
            get
            {
                return new DocumentClient(getApi());
            }
        }

        public ManagementClient ManagementClient
        {
            get
            {
                return new ManagementClient(getApi());
            }
        }

        public OptimizeClient OptimizeClient
        {
            get
            {
                return new OptimizeClient(getApi());
            }
        }

        public StampClient StampClient
        {
            get
            {
                return new StampClient(getApi());
            }
        }

        public ImageClient ImageClient
        {
            get
            {
                return new ImageClient(getApi());
            }
        }

        public OcrClient OcrClient
        {
            get
            {
                return new OcrClient(getApi());
            }
        }

        public JobClient JobClient
        {
            get
            {
                return new JobClient(getApi());
            }
        }

        public MergeClient MergeClient
        {
            get
            {
                return new MergeClient(getApi());
            }
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

        public HttpClient getApi()
        {


            HttpClient client;

            if (string.IsNullOrEmpty(_token))
            {
                ClientCredential clientCred = new ClientCredential(_clientId, _key);

                string token;

                AuthenticationContext authenticationContext = new AuthenticationContext(_authString, false);
                AuthenticationResult authenticationResult = authenticationContext.AcquireTokenAsync(_clientId, clientCred).ConfigureAwait(false).GetAwaiter().GetResult();
                token = authenticationResult.AccessToken;
                //txtToken.Text = token;


                // Do Stamp

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                client = new HttpClient();
                client.Timeout = new TimeSpan(0, 5, 0);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                //client.SetBearerToken(token);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));

                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Uri apiUri = new Uri(_api);
                client.BaseAddress = apiUri;

            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                client = new HttpClient();
                client.Timeout = new TimeSpan(0, 5, 0);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                //client.SetBearerToken(token);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Uri apiUri = new Uri(_api);
                client.BaseAddress = apiUri;
            }


            return client;
        }

        public string getApiToken()
        {

            //string tenantName = "devynooxlive.onmicrosoft.com";
            //string authString = "https://login.microsoftonline.com/" + tenantName;

            // SLApp
            //string clientId = "98a707a7-1860-4bbb-b956-51d95f1f338c";
            //string key = "o6YE76EHPPdnia7h/juHKIdDf7bWYgcu3PbzHuK6qJk=";


            //string resource = clientId;


            ClientCredential clientCred = new ClientCredential(_clientId, _key);

            string token;

            AuthenticationContext authenticationContext = new AuthenticationContext(_authString, false);
            AuthenticationResult authenticationResult = authenticationContext.AcquireTokenAsync(_clientId, clientCred).ConfigureAwait(false).GetAwaiter().GetResult();
            token = authenticationResult.AccessToken;
            //txtToken.Text = token;


            return token;

        }


    }
}
