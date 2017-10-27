using BACKnetLutron.BusinessEntities;
using BACKnetLutron.Services;
using BacnetLutronWeb.BackNet.BacNetAction;
using System;
using System.Collections.Generic;
using System.IO.BACnet;
using System.Linq;
using System.Threading;
using System.Web;

namespace BacnetLutronWeb.BackNet
{
    public static class BackNetClientInit
    {

        static BacnetClient bacNetClient;

        /// <summary>
        /// Start Bacnet protocol.
        /// </summary>
        public static void StartBackNetService()
        {
            if (bacNetClient == null)
            {
                BacnetIpUdpProtocolTransport newPort = new BacnetIpUdpProtocolTransport(0xBAC0, false);
                bacNetClient = new BacnetClient(newPort);

                //bacNetClient.Dispose();
                //   Thread.Sleep(1000);
                bacNetClient.Start();
                bacNetClient.OnIam += new BacnetClient.IamHandler(BacNetService.Handler_OmIam);
                bacNetClient.OnWhoIs += new BacnetClient.WhoIsHandler(BacNetService.handler_OnWhoIs);

                bacNetClient.WhoIs();
            }
            else
            {
                bacNetClient.OnIam -= new BacnetClient.IamHandler(BacNetService.Handler_OmIam);
                bacNetClient.OnWhoIs -= new BacnetClient.WhoIsHandler(BacNetService.handler_OnWhoIs);
            }
            Thread.Sleep(5000);
        }




        public static BacnetClient NewBackNetClient()
        {
            BacnetIpUdpProtocolTransport newPort = new BacnetIpUdpProtocolTransport(0xBAC0, false);
            bacNetClient = new BacnetClient(newPort);
            bacNetClient.Start();
            return bacNetClient;
        }


    }
}