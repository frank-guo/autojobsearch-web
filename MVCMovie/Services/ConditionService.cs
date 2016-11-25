using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Services
{
    public class ConditionService : IConditionService
    {
        private IRepository<Condition> repository;

        public ConditionService(IRepository<Condition>  repository) 
        {
            this.repository = repository;
        }

        public Condition GetById(int siteId)
        {
            var cond = repository.GetByID(siteId);

            if (cond == null)
            {
                return null;
            }

            var titleConds = new List<TitleCond>();
            var locationConds = new List<LocationCond>();
            foreach (TitleCond tc in cond.titleConds)
            {

                titleConds.Add(tc);
            }

            foreach (LocationCond lc in cond.locationConds)
            {

                locationConds.Add(lc);
            }

            var condition = new Condition();
            condition.titleConds = titleConds;
            condition.locationConds = locationConds;

            return condition;
        }
    }
}