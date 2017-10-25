using BACKnetLutron.BusinessEntities;
using BACKnetLutron.BusinessEntities.Obix;
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
    [RoutePrefix("api/ObixBacNet")]
    public class ObixBacNetController : System.Web.Http.ApiController
    {

        #region Coustructor
        public ObixBacNetController()
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
            var lightLevel = BackNetReadAction.SetConfLightLevel(lightLevelEntity);
            return Ok(lightLevel);
        }

        [HttpPost]
        [Route("SetConfLightScene")]
        public IHttpActionResult SetConfLightScene(LightSceneEntity lightSceneEntity)
        {
            var lightLevel = BackNetReadAction.SetConfLightScene(lightSceneEntity);
            return Ok(lightLevel);
        }


        [HttpPost]
        [Route("SetConfLightState")]
        public IHttpActionResult SetConfLightState(LightStateEntity lightStateEntity)
        {
            var lightState = BackNetReadAction.SetConfLightState(lightStateEntity);
            return Ok(lightState);
        }


        #endregion
    }
}