using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;

delegate Boolean CriteriaOperator(String field, String value);

namespace MVCMovie.Utils
{
    public class SearchCriteriaEvaluator
    {
        private SearchCriteria searchCriteria;
        private CriteriaOperator criteriaOperator;

        public SearchCriteria SearchCriteria
        {
            get
            {
                return searchCriteria;
            }

            set
            {
                searchCriteria = value;
                this.criteriaOperator = createOperator(searchCriteria.CriteriaOperator);
            }
        }

        public SearchCriteriaEvaluator ()
        {

        }

        public SearchCriteriaEvaluator(SearchCriteria criteria)
        {
            this.SearchCriteria = criteria;
            this.criteriaOperator = createOperator(criteria.CriteriaOperator);
        }



        private CriteriaOperator createOperator(String _operator)
        {
            switch (_operator) {
                case "equals":
                    {
                        return (String field, String value) =>
                        {
                            if (field == null || value == null)
                            {
                                return false;
                            }
                            if (field.Equals(value))
                            {
                                return true;
                            }
                            return false;
                        };
                    }
                case "contains":
                    {
                        return (String field, String value) =>
                        {
                            if (field == null || value == null)
                            {
                                return false;
                            }
                            if (field.Contains(value))
                            {
                                return true;
                            }
                            return false;
                        };
                    }
                default:
                    {
                        return (String field, String value) =>
                        {
                            return false;
                        };
                    }
            }
        }


        private string getFieldValue(RecruitingSite site, HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            if (site == null || commonAncestor == null || levelNoCommonAnstr<=0)
            {
                return null;
            }
            switch (SearchCriteria.FieldName)
            {
                case "Title":
                    {
                        return site.getJobTitleNode(commonAncestor, levelNoCommonAnstr).InnerText;
                    }
                case "Location":
                    {
                        return site.getOtherInfo(commonAncestor, levelNoCommonAnstr).InnerText;
                    }
                case "Company Name":
                    {
                        return site.getCompanyNameNode(commonAncestor, levelNoCommonAnstr).InnerText;
                    }
                default:
                    {
                        return "";
                    }
            }
        }

        public Boolean? evaluate (RecruitingSite site, HtmlElement commonAncestor, int levelNoAncestor)
        {
            if (SearchCriteria == null)
            {
                return null;
            }
            String fieldValue = getFieldValue(site, commonAncestor, levelNoAncestor);

            foreach(SearchCriteriaValue val in SearchCriteria.Values)
            {
                if (criteriaOperator(fieldValue, val.value))
                {
                    return true;
                }
            }

            return false;
            
        }
    }
}