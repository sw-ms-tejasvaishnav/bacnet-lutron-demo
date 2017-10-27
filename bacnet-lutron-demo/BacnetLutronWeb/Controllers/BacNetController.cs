using BACKnetLutron.BusinessEntities;
using BACKnetLutron.BusinessEntities.Common_Constant;
using BACKnetLutron.BusinessEntities.BacNetEntity;
using BACKnetLutron.Services;
using BacnetLutronWeb.BackNet;
using BacnetLutronWeb.BacNetAction;
using System;
using System.Collections.Generic;
using System.IO.BACnet;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BACKnetLutron.Controllers
{
    [RoutePrefix("api/BacNet")]
    public class BacNetController : System.Web.Http.ApiController
    {

        #region Coustructor
        public BacNetController()
        {
        }
        #endregion

        #region Controller Methods
        /// <summary>
        /// Start BacNet process.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("StartBackNetProtocol")]
        public IHttpActionResult StartBackNetProtocol()
        {
            BackNetClientInit.StartBackNetService();
            return Ok();
        }

        [HttpGet]
        [Route("GetConfLightState")]
        public IHttpActionResult GetConfLightState(Int32? deviceID)
        {
            var lightState = BackNetReadAction.GetConfLightState(deviceID);
            return Ok(lightState);
        }


        [HttpGet]
        [Route("GetConfLightLevel")]
        public IHttpActionResult GetConfLightLevel(Int32? deviceID)
        {
            var lightLevel = BackNetReadAction.GetConfLightLevel(deviceID);
            return Ok(lightLevel);
        }


        [HttpGet]
        [Route("GetConfLightingScene")]
        public IHttpActionResult GetConfLightingScene(Int32? deviceID)
        {
            var lightLevel = BackNetReadAction.GetConfLightingScene(deviceID);
            return Ok(lightLevel);
        }

        [HttpPost]
        [Route("SetConfLightLevel")]
        public IHttpActionResult SetConfLightLevel(LightLevelEntity lightLevelEntity)
        {
            var lightLevel = BackNetWriteAction.SetConfLightLevel(lightLevelEntity);
            return Ok(lightLevel);
        }

        [HttpPost]
        [Route("SetConfLightScene")]
        public IHttpActionResult SetConfLightScene(LightSceneEntity lightSceneEntity)
        {
            var lightLevel = BackNetWriteAction.SetConfLightScene(lightSceneEntity);
            return Ok(lightLevel);
        }


        [HttpPost]
        [Route("SetConfLightState")]
        public IHttpActionResult SetConfLightState(LightStateEntity lightStateEntity)
        {
            var lightState = BackNetWriteAction.SetConfLightState(lightStateEntity);
            return Ok(lightState);
        }


        [HttpPost]
        [Route("SetLightingScene")]
        public IHttpActionResult SetLightingScene(LightSceneEntity lightscene)
        {
            LightSceneEntity lightScenetemp = new LightSceneEntity();
            lightscene.Value = EnumConstants.GetEnumValueFromDescription<LightSceneEnum>(lightscene.LightScene).ToString();
            var lightScene = BackNetWriteAction.SetConfLightScene(lightscene);
            var lightLevel = BackNetReadAction.GetConfLightLevel(lightscene.DeviceID);
            var deviceDetail = new DeviceDetailEnity
            {
                DeviceID = lightScene.DeviceID,
                LightScene = lightScene.LightScene,
                LightSceneValue = lightScene.Value,
                LightLevel = lightLevel.LightLevel,
                //LightState = lightState.LightState
            };
            return Ok(deviceDetail);
        }


        [HttpPost]
        [Route("SetLightingLevel")]
        public IHttpActionResult SetLightingLevel(LightLevelEntity lightLevel)
        {
            LightSceneEntity lightScenetemp = new LightSceneEntity();
            var deviceLightLevel = BackNetWriteAction.SetConfLightLevel(lightLevel);
            var lightScene = BackNetReadAction.GetConfLightingScene(lightLevel.DeviceID);
            var deviceDetail = new DeviceDetailEnity
            {
                DeviceID = lightScene.DeviceID,
                LightScene = lightScene.LightScene,
                LightSceneValue = lightScene.Value,
                LightLevel = deviceLightLevel.LightLevel,
                //LightState = lightState.LightState
            };
            return Ok(deviceDetail);
        }
        #endregion
    }
}