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
        void Delete(int id);
        void Delete(IList<PathNode> job1);
    }
}
