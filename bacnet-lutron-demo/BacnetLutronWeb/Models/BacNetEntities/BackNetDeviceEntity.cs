using System;
using System.Collections.Generic;
using System.IO.BACnet;
using System.Linq;
using System.Web;

namespace BACKnetLutron.BusinessEntities
{
    public class BackNetDeviceEntity
    {
        public BacnetAddress BacNetAddress { get; set; }

        public uint DeviceId { get; set; }

        public uint InstanceId { get; set; }
    }
}