using Microsoft.AspNetCore.Mvc;
using Netcoreaaa.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netcoreaaa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : Controller
    {

        //localhost/api/warehouse/getwarehouse
        [HttpGet("GetWarehouse")]
        public IActionResult GetWearehouse()
        {
            return Ok("a");
        }

        [HttpGet("GetWarehouses")]
        public IActionResult GetWearehouses([FromServices] IWarehouseService _warehouseService)
        {
            return Ok(_warehouseService.GetWarehouses());
        }
        //localhost/api/warehouse/getwarehousefromquery?userID=1
        //por defecto lo toma como querystring
        [HttpGet("GetWearehouseFromQuery")]
        public IActionResult GetWearehouseFromQuery(int warehouseID, string warehouseName) //para que sea explicito ponemos [FromQuery] antes de la declaracion
        {
            return Ok($"a query {warehouseID} {warehouseName}");
        }

        [HttpGet("GetWarehouseFromRoute/{warehouseID}/{warehouseName}")]
        public IActionResult GetWarehouseFromRoute(int warehouseID, string warehouseName) //se puede poner [FromRoute]
        {
            return Ok($"From route ID: {warehouseID}: {warehouseName}");
        }

        [HttpPost("CreateWarehouse")]
        public IActionResult CreateWarehouse(Warehouse warehouse) //[FromBody]
        {
            return Ok($"POST XD: {warehouse.WarehouseID}: {warehouse.WarehouseName}");
        }
    }
}
