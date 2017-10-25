﻿using BACKnetLutron.BusinessEntities;
using BACKnetLutron.BusinessEntities.Obix;
using BACKnetLutron.Services;
using BacnetLutronWeb.BackNet.BacNetAction;
using BacnetLutronWeb.DataModal;
using System;
using System.Collections.Generic;
using System.IO.BACnet;
using System.Linq;
using System.Threading;
using System.Web;

namespace BacnetLutronWeb.BacNetAction
{
    public static class BackNetReadAction
    {

        static BacnetClient bacNetClient;
        static BackNetDeviceModel bacNetDeviceModel = new BackNetDeviceModel();
        static BackNet_LutronEntities lutronEntities = new BackNet_LutronEntities();
        public static LightStateEntity GetConfLightState(int? deviceID)
        {
       
            var bacnetDeviceFromDB = lutronEntities.BACnetDevices
                                       .Where(x => x.device_id == deviceID
                                            && x.object_instance == (int?)LutronObjectType.Lighting_State)
                                       .Select(x => x).FirstOrDefault();


            IList<BacnetValue> loBacnetValueList;
            BacnetAddress loBacnetAddress;
            loBacnetAddress = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.network_id);

            loBacnetAddress.RoutedSource = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.routed_source,
                (ushort)bacnetDeviceFromDB.routed_net);

            bacNetClient.ReadPropertyRequest(loBacnetAddress, new BacnetObjectId(BacnetObjectTypes.OBJECT_BINARY_VALUE,
                (uint)LutronObjectType.Lighting_State), BacnetPropertyIds.PROP_PRESENT_VALUE, out loBacnetValueList);

            if (loBacnetValueList != null && loBacnetValueList.Count > 0)
            {
                return new LightStateEntity { DeviceID = (Int32)deviceID, LightState = Convert.ToBoolean(loBacnetValueList.FirstOrDefault().Value) };
            }
            else
            {
                return new LightStateEntity { DeviceID = (Int32)deviceID, LightState = null };
            }
        }


        public static LightLevelEntity GetConfLightLevel(int? deviceID)
        {
            
            var bacnetDeviceFromDB = lutronEntities.BACnetDevices
                                       .Where(x => x.device_id == deviceID
                                           && x.object_instance == (int?)LutronObjectType.Lighting_Level)
                                       .Select(x => x).FirstOrDefault();


            IList<BacnetValue> bacnetValueList;
            BacnetAddress bacnetAddress;
            bacnetAddress = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.network_id);

            bacnetAddress.RoutedSource = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.routed_source,
                (ushort)bacnetDeviceFromDB.routed_net);

            bacNetClient.ReadPropertyRequest(bacnetAddress, new BacnetObjectId(BacnetObjectTypes.OBJECT_ANALOG_VALUE,
                (uint)LutronObjectType.Lighting_Level), BacnetPropertyIds.PROP_PRESENT_VALUE, out bacnetValueList);

            if (bacnetValueList != null && bacnetValueList.Count > 0)
            {
                return new LightLevelEntity { DeviceID = (Int32)deviceID, LightLevel = Convert.ToString(bacnetValueList.FirstOrDefault().Value) };
            }
            else
            {
                return new LightLevelEntity { DeviceID = (Int32)deviceID, LightLevel = string.Empty };
            }
        }


        public static LightSceneEntity GetConfLightingScene(int? deviceID)
        {
            LightSceneEntity lightSceneEntity = new LightSceneEntity();            
            var bacnetDeviceFromDB = lutronEntities.BACnetDevices
                                       .Where(x => x.device_id == deviceID
                                           && x.object_instance == (int?)LutronObjectType.Lighting_Scene)
                                       .Select(x => x).FirstOrDefault();


            IList<BacnetValue> bacnetValueList;
            BacnetAddress bacnetAddress;
            bacnetAddress = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.network_id);

            bacnetAddress.RoutedSource = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.routed_source,
                (ushort)bacnetDeviceFromDB.routed_net);

            bacNetClient.ReadPropertyRequest(bacnetAddress, new BacnetObjectId(BacnetObjectTypes.OBJECT_MULTI_STATE_VALUE,
                (uint)LutronObjectType.Lighting_Scene), BacnetPropertyIds.PROP_PRESENT_VALUE, out bacnetValueList);

            
            var lightLevelelement = ObixReadAction.ReadLightScene();

            if (lightLevelelement != null)
            {
                lightSceneEntity.LightScene = lightLevelelement.Attribute("val").Value;
            }


            if (bacnetValueList != null && bacnetValueList.Count > 0)
            {
                lightSceneEntity.Value = Convert.ToString(bacnetValueList.FirstOrDefault().Value);
            }

            lightSceneEntity.DeviceID = (Int32)deviceID;

            return lightSceneEntity;
        }



        public static LightLevelEntity SetConfLightLevel(LightLevelEntity lightLevelEntity)
        {
            
            var bacnetDeviceFromDB = lutronEntities.BACnetDevices
                                       .Where(x => x.device_id == lightLevelEntity.DeviceID
                                           && x.object_instance == (int?)LutronObjectType.Lighting_Level)
                                       .Select(x => x).FirstOrDefault();

            if (bacnetDeviceFromDB != null && bacnetDeviceFromDB.bacnet_device_id > 0)
            {
                BacnetAddress bacnetAddress = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.network_id);
                bacnetAddress.RoutedSource = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.routed_source,
                    (ushort)bacnetDeviceFromDB.routed_net);


                BacnetValue newLightLevel = new BacnetValue(BacnetApplicationTags.BACNET_APPLICATION_TAG_REAL, Convert.ToSingle(lightLevelEntity.LightLevel));
                BacnetValue[] writeNewLightLevel = { newLightLevel };

                bacNetClient.WritePropertyRequest(bacnetAddress,
                    new BacnetObjectId(BacnetObjectTypes.OBJECT_ANALOG_VALUE, (uint)LutronObjectType.Lighting_Level),
                    BacnetPropertyIds.PROP_PRESENT_VALUE, writeNewLightLevel);

            }



            return GetConfLightLevel(lightLevelEntity.DeviceID);
        }



        public static LightSceneEntity SetConfLightScene(LightSceneEntity lightSceneEntity)
        {
           
            var bacnetDeviceFromDB = lutronEntities.BACnetDevices
                                       .Where(x => x.device_id == lightSceneEntity.DeviceID
                                           && x.object_instance == (int?)LutronObjectType.Lighting_Scene)
                                       .Select(x => x).FirstOrDefault();

            if (bacnetDeviceFromDB != null && bacnetDeviceFromDB.bacnet_device_id > 0)
            {
                BacnetAddress bacnetAddress = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.network_id);
                bacnetAddress.RoutedSource = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.routed_source,
                    (ushort)bacnetDeviceFromDB.routed_net);


                BacnetValue newLightScene = new BacnetValue(BacnetApplicationTags.BACNET_APPLICATION_TAG_UNSIGNED_INT, Convert.ToUInt32(lightSceneEntity.Value));
                BacnetValue[] writeNewLightScene = { newLightScene };

                bacNetClient.WritePropertyRequest(bacnetAddress,
                    new BacnetObjectId(BacnetObjectTypes.OBJECT_MULTI_STATE_VALUE, (uint)LutronObjectType.Lighting_Scene),
                    BacnetPropertyIds.PROP_PRESENT_VALUE, writeNewLightScene);

            }

            Thread.Sleep(1000);

            return GetConfLightingScene(lightSceneEntity.DeviceID);
        }


        public static LightStateEntity SetConfLightState(LightStateEntity lightStateEntity)
        {
            
            var bacnetDeviceFromDB = lutronEntities.BACnetDevices
                                       .Where(x => x.device_id == lightStateEntity.DeviceID
                                           && x.object_instance == (int?)LutronObjectType.Lighting_State)
                                       .Select(x => x).FirstOrDefault();

            if (bacnetDeviceFromDB != null && bacnetDeviceFromDB.bacnet_device_id > 0)
            {
                BacnetAddress bacnetAddress = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.network_id);
                bacnetAddress.RoutedSource = new BacnetAddress(BacnetAddressTypes.IP, bacnetDeviceFromDB.routed_source,
                    (ushort)bacnetDeviceFromDB.routed_net);


                BacnetValue newLightState = new BacnetValue(BacnetApplicationTags.BACNET_APPLICATION_TAG_ENUMERATED, lightStateEntity.LightState == true ? 1 : 0);
                BacnetValue[] writeNewLightState = { newLightState };

                bacNetClient.WritePropertyRequest(bacnetAddress,
                    new BacnetObjectId(BacnetObjectTypes.OBJECT_BINARY_VALUE, (uint)LutronObjectType.Lighting_State),
                    BacnetPropertyIds.PROP_PRESENT_VALUE, writeNewLightState);

            }

            Thread.Sleep(1000);

            return GetConfLightState(lightStateEntity.DeviceID);
        }

    }
}