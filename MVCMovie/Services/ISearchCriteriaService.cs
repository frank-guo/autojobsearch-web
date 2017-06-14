using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCMovie.Services
{
    public interface ISearchCriteriaService
    {
        IList<SearchCriteriaViewModel> Get(int siteId);
        void Update(IList<SearchCriteria> searchRule);
        void Delete(int id);
        void Delete(IList<SearchCriteria> searchRule);
    }
}