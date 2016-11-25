using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCMovie.Services
{
    public interface INextPathService
    {
        void Update(NextPosition nextPosition);
        void Update(IList<int> nextPath, int siteId);
        void Delete(int id);
        void Delete(IList<NextPosition> job1);
    }
}
