using BACKnetLutron.Init.Obix;
using NetBIX.oBIX.Client.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BacnetLutronWeb.BackNet.BacNetAction
{
    
    public static class ObixReadAction
    {
        public static string obixHubURL = ConfigurationManager.AppSettings["ObixHubURL"];


        public static XElement ReadLightScene()
        {
            ObixClientInit obixClientInit = new ObixClientInit();
            ObixResult<XElement> xmlLevelResult = obixClientInit.oBixClient.ReadUriXml(new Uri(obixHubURL + ConfigurationManager.AppSettings["LightScene"]));

            IEnumerable<XNode> lighLevelLst = xmlLevelResult.Result.Document.DescendantNodes();
            XElement lightLevelelement = lighLevelLst.LastOrDefault() as XElement;
            return lightLevelelement;
        }
    }
}