using BACKnetLutron.BusinessEntities;
using System.Collections.Generic;

namespace BACKnetLutron.Services
{
    public class BackNetDeviceModel
    {
        public BackNetDeviceModel()
        {
            BACnetDeviceList = new List<BackNetDeviceEntity>();
        }
        public List<BackNetDeviceEntity> BACnetDeviceList { get; set; }
    }
}