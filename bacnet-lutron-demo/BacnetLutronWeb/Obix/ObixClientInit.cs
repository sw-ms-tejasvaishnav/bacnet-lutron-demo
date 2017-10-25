using NetBIX.oBIX.Client;
using NetBIX.oBIX.Client.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace BACKnetLutron.Init.Obix
{
    public class ObixClientInit
    {
        public XmlObixClient oBixClient;
        public ObixResult oBixResult;

        public ObixClientInit()
        {
            string obixHubURL = ConfigurationManager.AppSettings["ObixHubURL"];

            oBixClient = new XmlObixClient(new Uri(obixHubURL));
            oBixClient.WebClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("obix:obix")));

            oBixResult = oBixClient.Connect();
            if (oBixResult != ObixResult.kObixClientSuccess)
            {
                throw new InvalidOperationException();
            }
        }
    }
}