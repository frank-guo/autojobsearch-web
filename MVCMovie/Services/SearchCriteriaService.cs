using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Services
{
    public class SearchCriteriaService : ISearchCriteriaService
    {
        private IRepository<SearchCriteria> repository;
        private IRepository<RecruitingSite> recruitingSiteRepository;

        public SearchCriteriaService(IRepository<SearchCriteria> repository,
                IRepository<RecruitingSite> recruitingSiteRepository)
        {
            this.repository = repository;
            this.recruitingSiteRepository = recruitingSiteRepository;
        }

        public IList<SearchCriteriaViewModel> Get(int siteId)
        {
            if (siteId <= 0)
            {
                return null;
            }
            RecruitingSite site = recruitingSiteRepository.GetByID(siteId);

            //ToDo: use some model mapping library to do model mapping
            IList<SearchCriteriaViewModel> rule = new List<SearchCriteriaViewModel>();
            foreach(SearchCriteria criteria in site.SearchRule) {
                var criteriaVM = new SearchCriteriaViewModel();
                criteriaVM.id = criteria.ID;
                criteriaVM.recruitingSiteId = criteria.RecruitingSiteId;
                criteriaVM.fieldName = criteria.FieldName;
                criteriaVM._operator = criteria.CriteriaOperator;

                IList<string> values = new List<string>();
                foreach(SearchCriteriaValue value in criteria.Values)
                {
                    values.Add(value.value);
                }
                criteriaVM.values = values;

                rule.Add(criteriaVM);
            }

            return rule;
        }

        public void Update(IList<SearchCriteriaViewModel> searchRule, int id)
        {
            if (searchRule == null)
            {
                return;
            }

            RecruitingSite site = recruitingSiteRepository.GetByID(id);
            IList<SearchCriteria> searchCriterias = site.SearchRule;
            Delete(searchCriterias);

            foreach (SearchCriteriaViewModel criteriaVM in searchRule)
            {
                var criteria = new SearchCriteria();
                criteria.FieldName = criteriaVM.fieldName != null ? criteriaVM.fieldName : null;
                criteria.CriteriaOperator = criteriaVM._operator != null ? criteriaVM._operator : null;

                IList<SearchCriteriaValue> values = new List<SearchCriteriaValue>();
                foreach(string val in criteriaVM.values) {
                    var value = new SearchCriteriaValue();
                    value.value = val;
                    values.Add(value);
                }
                criteria.Values = values;
                searchCriterias.Add(criteria);
            }
            recruitingSiteRepository.Update(site);
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                return;
            }
            repository.Delete(id);
        }

        public void Delete(IList<SearchCriteria> searchRule)
        {
            if (searchRule == null || searchRule.Count == 0)
            {
                return;
            }

            foreach (SearchCriteria criteria in searchRule)
            {
                Delete(criteria.ID);
            }
        }
    }
}