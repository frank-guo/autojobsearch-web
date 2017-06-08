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
            var rule = new[]
            {
                new {
                    id = 1,
                    fieldName = "City",
                    _operator = "equal",
                    values = vals
                },
                new {
                    id = 2,
                    fieldName = "City",
                    _operator = "equal",
                    values = new [] { "Burnaby", "Richmond" }
                }
            };

            return Ok(rule);
        }
    }
}
