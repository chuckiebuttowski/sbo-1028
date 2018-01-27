using sbo.fx;
using sbo.fx.Factories;
using sbo.fx.Interfaces;
using sbo.fx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SBOClient.Controllers.SboControllers
{
    /// <summary>
    /// This end point is for accessing warehouses from SAP
    /// </summary>
    [RoutePrefix("api/warehouses")]
    public class WarehouseController : ApiController
    {
        IWarehouseRepository repo = new RepositoryFactory().WarehouseRepository();
        string errMsg = "";

        public WarehouseController()
        {
            repo.InitRepository(GlobalInstance.Instance.SqlObject);
        }

        /// <summary>
        /// Gets all warehouses from SAP
        /// </summary>
        /// <returns>List of warehouses</returns>
        [Route("get-warehouses")]
        [HttpGet]
        public async Task<List<oWarehouse>> GetWarehouses()
        {
            try
            {
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed)
                {
                    errMsg = "Unable to connect to server.";
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "No Server Connection";
                    throw new HttpResponseException(resp);
                }

                return await repo.GetList(null);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets a warehouse filtered by warehouse code 
        /// </summary>
        /// <param name="warehouseCode"></param>
        /// <returns>Warehouse</returns>
        [Route("get-warehouse")]
        [HttpGet]
        public async Task<oWarehouse> GetWarehouse(string warehouseCode)
        {
            try
            {
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed)
                {
                    errMsg = "Unable to connect to server.";
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "No Server Connection";
                    throw new HttpResponseException(resp);
                }

                var list = await repo.GetList(null);

                return list.FirstOrDefault(x=> x.WarehouseCode == warehouseCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }

        /// <summary>
        /// Gets a bin location filtered by bin code
        /// </summary>
        /// <param name="binCode"></param>
        /// <returns>Bin Location</returns>
        [Route("get-bin-location")]
        [HttpGet]
        public async Task<oBin> GetBinLocation(string binCode)
        {
            try
            {
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed)
                {
                    errMsg = "Unable to connect to server.";
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "No Server Connection";
                    throw new HttpResponseException(resp);
                }

                return await repo.GetBinLocation(binCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }

        /// <summary>
        /// Gets list of bin locations
        /// </summary>
        /// <returns>List of Bin Locations</returns>
        [Route("get-bin-locations")]
        [HttpGet]
        public async Task<List<oBin>> GetBinLocations()
        {
            try
            {
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed)
                {
                    errMsg = "Unable to connect to server.";
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "No Server Connection";
                    throw new HttpResponseException(resp);
                }

                return await repo.GetBinLocations(null);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }

        /// <summary>
        /// Gets list of bin locations per warehouse
        /// </summary>
        /// <param name="warehouseCode"></param>
        /// <returns>List of Bin Locations</returns>
        [Route("get-bin-locations-per-warehouse")]
        [HttpGet]
        public async Task<List<oBin>> GetBinLocationsPerWarehouse(string warehouseCode)
        {
            try
            {
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed)
                {
                    errMsg = "Unable to connect to server.";
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "No Server Connection";
                    throw new HttpResponseException(resp);
                }

                return await repo.GetBinLocations(x=> x.WarehouseCode == warehouseCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }
    }
}
