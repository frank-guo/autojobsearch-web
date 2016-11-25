using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCMovie.Services
{
    public interface ICompanyPathService
    {
        IList<Company> Get(int siteId);
        void Update(IList<int> companyPath, int siteId);
        void Delete(int id);
        void Delete(IList<Company> companyPath);
    }
}
