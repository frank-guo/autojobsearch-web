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

        public void Update(ConditionViewModel conditionVM)
        {
            var condID = conditionVM.ID;
            repository.Delete(conditionVM.ID);

            var titleConds = conditionVM.titleConds;
            var locationConds = conditionVM.locationConds;

            var condition = new Condition();
            condition.titleConds = new List<TitleCond>();
            condition.locationConds = new List<LocationCond>();
            condition.ID = condID;

            if (titleConds != null)
            {
                for (int i = 0; i < titleConds.Count; i++)
                {
                    var tc = new TitleCond
                    {
                        titleCond = titleConds.ElementAt(i)
                    };
                    condition.titleConds.Add(tc);
                }
            }

            if (locationConds != null)
            {
                for (int i = 0; i < locationConds.Count; i++)
                {
                    var lc = new LocationCond
                    {
                        locationCond = locationConds.ElementAt(i)
                    };
                    condition.locationConds.Add(lc);
                }
            }

            repository.Insert(condition);
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