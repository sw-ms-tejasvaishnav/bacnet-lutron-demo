using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BACKnetLutron.BusinessEntities.Obix
{
    public class LightLevelEntity
    {
        public Int32 DeviceID { get; set; }
        public string LightLevel { get; set; }
    }

    public class LightStateEntity
    {
        public Int32 DeviceID { get; set; }
        public bool? LightState { get; set; }
    }

    public class LightSceneEntity
    {
        public Int32 DeviceID { get; set; }
        public string LightScene { get; set; }
        public string Value { get; set; }
    }

    public class DeviceDetailEnity
    {
        public Int32 DeviceID { get; set; }
        public string LightLevel { get; set; }
        public bool? LightState { get; set; }
        public string LightScene { get; set; }
        public string LightSceneValue { get; set; }
    }
}