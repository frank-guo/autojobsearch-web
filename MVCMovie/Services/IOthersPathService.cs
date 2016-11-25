using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCMovie.Services
{
    public interface IOthersPathService
    {
        IList<Others> Get(int siteId);
        void Update(IList<int> othersPath, int id);
        void Delete(int id);
        void Delete(IList<Others> job1);
    }
}
