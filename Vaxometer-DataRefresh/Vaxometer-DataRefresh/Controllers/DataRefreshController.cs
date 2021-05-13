using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vaxometer_DataRefresh.Manager;
using Vaxometer_DataRefresh.Repository;

namespace Vaxometer_DataRefresh.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DataRefreshController : ControllerBase
    {
        private readonly IVexoManager _vexoManager;
        public DataRefreshController(IVexoManager vexoManager)
        {
            _vexoManager = vexoManager;
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
    }
}