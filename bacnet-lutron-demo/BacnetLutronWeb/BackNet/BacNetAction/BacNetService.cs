using BACKnetLutron.BusinessEntities;
using BACKnetLutron.Services;
using BacnetLutronWeb.DataModal;
using System;
using System.Collections.Generic;
using System.IO.BACnet;
using System.Linq;
using System.Web;

namespace BacnetLutronWeb.BackNet.BacNetAction
{
    public static class BacNetService
    {

        static BackNet_LutronEntities dbContext = new BackNet_LutronEntities();
        static BacnetClient bacNetClient;
        static BackNetDeviceModel bacNetDeviceModel = new BackNetDeviceModel();
        /// <summary>
        /// Gets available device and create global list
        /// </summary>
        /// <param name="bacNetClient">Passes bacNetClient detail</param>
        /// <param name="bacNetAddress">Passes Address</param>
        /// <param name="deviceId">Passes device id.</param>
        /// <param name="maxPLoad">Passes maxpayload.</param>
        /// <param name="segmentation"></param>
        /// <param name="vendorId"></param>
        public static void Handler_OmIam(BacnetClient bacNetClient, BacnetAddress bacNetAddress, uint deviceId, uint maxPLoad,
            BacnetSegmentations segmentation, ushort vendorId)
        {
            if (bacNetDeviceModel != null)
            {
                //// OnIam get current device and add into list to process bunch of device in DBs
                lock (bacNetDeviceModel.BACnetDeviceList)
                {

                    if (!bacNetDeviceModel.BACnetDeviceList.Any(x => x.DeviceId == deviceId))
                    {
                        //// Not already in the list

                        bacNetDeviceModel.BACnetDeviceList.Add(new BackNetDeviceEntity
                        {
                            BacNetAddress = bacNetAddress,
                            DeviceId = deviceId,
                            InstanceId = 0
                        });   //// add it
                    }
                }

                if (bacNetDeviceModel.BACnetDeviceList.Count == 48)
                {
                     BacNetService.AddBackNetDeviceDetail();
                }
            }
        }


        /// <summary>
        /// handler to assign alarm enrolment instance on event notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="adr"></param>
        /// <param name="low_limit"></param>
        /// <param name="high_limit"></param>
        public static void handler_OnWhoIs(BacnetClient sender, BacnetAddress adr, int low_limit, int high_limit)
        {
            if (low_limit != -1 && 1 < low_limit)
                return;
            else if (high_limit != -1 && 1 > high_limit)
                return;

            //int? alaramEnrollmentVal = _LutronLightRepository.GetAlaramEnrollment();
            //if (alaramEnrollmentVal != null)
            //{
            //    sender.Iam((uint)alaramEnrollmentVal, new BacnetSegmentations());
            //}
        }

        /// <summary>
        /// Adds bacnet device details.
        /// </summary>
        private static  void AddBackNetDeviceDetail()
        {
            if (bacNetDeviceModel != null && bacNetDeviceModel.BACnetDeviceList != null)
            {               
                List<BACnetDevice> bACnetDeviceLst = new List<BACnetDevice>();
                List<BACnetDeviceMapping> bACnetDeviceMappingLst = new List<BACnetDeviceMapping>();
                foreach (var deviceDetail in bacNetDeviceModel.BACnetDeviceList)
                {
                    IList<BacnetValue> objValueLst;
                    bacNetClient.ReadPropertyRequest(deviceDetail.BacNetAddress,
                        new BacnetObjectId(BacnetObjectTypes.OBJECT_DEVICE, deviceDetail.DeviceId),
                        BacnetPropertyIds.PROP_OBJECT_LIST, out objValueLst);
                    foreach (var objValue in objValueLst)
                    {

                        var isExistNetworkId = CheckIfExistNetworkAddress(deviceDetail.BacNetAddress.ToString(),
                            (int)((BacnetObjectId)objValue.Value).Instance, (int)deviceDetail.DeviceId
                            , ((BacnetObjectId)objValue.Value).Type.ToString());
                        if (isExistNetworkId == true)
                        {
                            continue;
                        }
                        IList<BacnetValue> objNameList;
                        bacNetClient.ReadPropertyRequest(deviceDetail.BacNetAddress,
                            new BacnetObjectId((BacnetObjectTypes)((BacnetObjectId)objValue.Value).Type,
                            ((BacnetObjectId)objValue.Value).Instance),
                            BacnetPropertyIds.PROP_OBJECT_NAME, out objNameList);
                        if (deviceDetail.BacNetAddress.RoutedSource != null && deviceDetail.BacNetAddress.RoutedSource.net != null)
                        {
                            var bacNetdevice = new BACnetDevice
                            {
                                device_id = Convert.ToInt32(deviceDetail.DeviceId),
                                network_id = deviceDetail.BacNetAddress.ToString(),
                                object_type = ((BacnetObjectId)objValue.Value).type.ToString(),
                                object_instance = Convert.ToInt32(((BacnetObjectId)objValue.Value).Instance.ToString()),
                                object_name = objNameList != null && objNameList.Count > 0 ? objNameList[0].Value.ToString() : null,
                                routed_source = deviceDetail.BacNetAddress.RoutedSource.ToString(),
                                routed_net = deviceDetail.BacNetAddress.RoutedSource.net
                            };
                            bACnetDeviceLst.Add(bacNetdevice);
                        }
                        int? suiteID = null, roomID = null;
                        var objName = Enum.GetName(typeof(LutronFloorObjectType), LutronFloorObjectType.OBJECT_ANALOG_VALUE).ToString();
                        if (((BacnetObjectId)objValue.Value).type.ToString().ToUpper() == objName)
                        {
                            if (Convert.ToInt32(((BacnetObjectId)objValue.Value).Instance.ToString()) < 4)
                            {
                                suiteID = 1;
                            }
                            else
                            {
                                suiteID = 2;
                            }

                            roomID = Convert.ToInt32(((BacnetObjectId)objValue.Value).Instance.ToString());
                        }
                        var bACnetDeviceMapping = new BACnetDeviceMapping
                        {
                            device_id = Convert.ToInt32(deviceDetail.DeviceId),
                            floor_id = Convert.ToInt32(deviceDetail.DeviceId),
                            suite_id = suiteID,
                            room_id = roomID,
                            object_instance = Convert.ToInt32(((BacnetObjectId)objValue.Value).Instance.ToString())
                        };
                        bACnetDeviceMappingLst.Add(bACnetDeviceMapping);
                    }

                }

                if (bACnetDeviceLst.Count() > 0)
                {
                    AddBacNetDeviceDetail(bACnetDeviceLst);
                }
                if (bACnetDeviceMappingLst.Count() > 0)
                {
                    AddBacNetMappingDetail(bACnetDeviceMappingLst);
                }
            }
        }


        public static bool CheckIfExistNetworkAddress(string networkIp, int instanceId, int deviceId, string objType)
        {
            var isExist = dbContext.BACnetDevices.Where(x => x.network_id == networkIp && x.object_instance == instanceId
            && x.object_type == objType && x.device_id == deviceId).Any();
            return isExist;
        }


        /// <summary>
        /// Adds bacnet device details on start process.
        /// </summary>
        /// <param name="bacNetDeviceLst">Passes list of devices.</param>
        public static async void AddBacNetDeviceDetail(List<BACnetDevice> bacNetDeviceLst)
        {
            if (bacNetDeviceLst.Count() > 0)
            {
                foreach (var bacNetDevice in bacNetDeviceLst)
                {
                    dbContext.BACnetDevices.Add(bacNetDevice);
                }
                await dbContext.SaveChangesAsync();
               // dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Adds bacnet mapping details on start process.
        /// </summary>
        /// <param name="bacNetDeviceMappingLst">Passes bacnet device mapping list.</param>
        public static async void AddBacNetMappingDetail(List<BACnetDeviceMapping> bacNetDeviceMappingLst)
        {
            if (bacNetDeviceMappingLst.Count() > 0)
            {
                foreach (var bacNetDeviceMap in bacNetDeviceMappingLst)
                {
                    dbContext.BACnetDeviceMappings.Add(bacNetDeviceMap);
                }
                await dbContext.SaveChangesAsync();
               // dbContext.SaveChanges();
            }
        }


    }
}