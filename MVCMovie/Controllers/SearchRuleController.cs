using System;
using MVCMovie.Models;
using MVCMovie.Services;
using System.Web.Http;
using System.Collections.Generic;

namespace MVCMovie.Controllers
{
    [RoutePrefix("api/searchrule")]
    public class SearchRuleController : ApiController
    {

        private ISearchCriteriaService searchCriteriaService;

        public SearchRuleController(ISearchCriteriaService searchCriteriaService)
        {
            this.searchCriteriaService = searchCriteriaService;
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var rule = searchCriteriaService.Get(id);
            return Ok(rule);
        }

        [Route("{id}")]
        [HttpPost]
        public IHttpActionResult Save(IList<SearchCriteria> searchRule)
        {
            if (searchRule == null)
            {
                var response = new Common.Model.Message()
                {
                    msgCode = Common.Model.MessageCode.InvlidModel,
                    message = Resources.Common.InvlidModel,
                };
                return Ok(response);
            }

            if (ModelState.IsValid)
            {
                searchCriteriaService.Update(searchRule);

                var response = new Common.Model.Message()
                {
                    msgCode = Common.Model.MessageCode.Success,
                    message = String.Format(Resources.Common.SaveSuccessMsg, Resources.SearchRule.searchRule),
                };
                return Ok(response);
            }
            else
            {

                var message = String.Format(Resources.Common.InvlidModel, Resources.SearchRule.searchRule);
                return BadRequest(message);
            }
        }
    }
}
