using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCMovie.Services
{
    public interface IPathNodeService
    {
        IList<PathNode> Get(int siteId);
        void Update(IList<PathNode> job1Path, int id);
        void Delete(int id);
        void Delete(IList<PathNode> job1);
    }
}
