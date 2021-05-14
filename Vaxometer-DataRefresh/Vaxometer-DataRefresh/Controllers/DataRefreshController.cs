using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vaxometer.Servicebus.Messages;
using Vaxometer.Servicebus.Publishers;
using Vaxometer_DataRefresh.Manager;
using Vaxometer_DataRefresh.Repository;

namespace Vaxometer_DataRefresh.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DataRefreshController : ControllerBase
    {
        private readonly IVexoManager _vexoManager;
        private readonly IMessagePublisher _messagePublisher;
        public DataRefreshController(IVexoManager vexoManager, IMessagePublisher messagePublisher)
        {
            _vexoManager = vexoManager;
            _messagePublisher = messagePublisher;
        }

        [HttpPost("RefreshAllDistricts")]
        public async Task<IActionResult> RefreshAllDistricts()
        {
            var response = await _vexoManager.RefreshAllDistricts();
            return Ok(response);
        }

        /// <summary>Protected API. Requires API Key</summary>
        [HttpPost("Refresh/{districtCode}")]
        public async Task<IActionResult> Upsert(int districtCode)
        {
            //if center code is BLR, then refresh 
            //TODO: if centerCode is other than BLR
            var response = await _vexoManager.RefershData(districtCode);
           
            return Ok(response);
        }

        [HttpPost("TestNotification/{id}")]
        public async Task<IActionResult> TestNotify(int id)
        {
            //if center code is BLR, then refresh 
            //TODO: if centerCode is other than BLR
            var ErrorMessage = string.Empty;
            var message = new ChangeStream()
            {
                CenterId = id,
                DistrictName = "BBMP",
                PinCode = 799002
            };
            try
            {
                await _messagePublisher.PublisherAsync<ChangeStream>(message);
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            if (ErrorMessage != string.Empty)
                return Ok(ErrorMessage);
            
            return Ok();
        }
    }
}