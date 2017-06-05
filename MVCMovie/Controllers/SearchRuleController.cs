using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCMovie.Controllers
{
    [RoutePrefix("api/searchrule")]
    public class SearchRuleController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            string[] vals = { "Vancouver" };
            var rule = new
            {
                fieldName = "City",
                operatori = "equal",
                values = vals
            };

            return Ok(rule);
        }
    }
}
