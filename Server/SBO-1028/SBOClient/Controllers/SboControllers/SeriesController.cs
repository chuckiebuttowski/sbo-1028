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
    /// This end point is for accessing numbering series from SAP
    /// </summary>
    [RoutePrefix("api/series")]
    public class SeriesController : ApiController
    {
        IRepository<oSeries> repo = new RepositoryFactory().SeriesRepository();
        string errMsg = "";

        public SeriesController()
        {
            repo.InitRepository(GlobalInstance.Instance.SqlObject);
        }

        /// <summary>
        /// Gets all numbering series filtered by object type
        /// </summary>
        /// <returns></returns>
        [Route("get-series")]
        [HttpGet]
        public async Task<List<oSeries>> GetSeries(SboTransactionType objectType)
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

                return await repo.GetList(x=> x.ObjectCode == ((int)objectType).ToString());
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets a numbering series from SAP
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="seriesCode"></param>
        /// <returns>Numbering Series</returns>
        [Route("get-series-by-code")]
        [HttpGet]
        public async Task<oSeries> GetSeriesByCode(SboTransactionType objectType, int seriesCode)
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

                var list = await repo.GetList(x => x.ObjectCode == ((int)objectType).ToString());
                return list.FirstOrDefault(x => x.Series == seriesCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
