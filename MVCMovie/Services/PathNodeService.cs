using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Services
{
    public class PathNodeService : IPathNodeService
    {
        private IRepository<PathNode> repository;

        public PathNodeService(IRepository<PathNode> repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                return;
            }
            repository.Delete(id);
        }

        public void Delete(IList<PathNode> job1)
        {
            if (job1 == null || job1.Count == 0)
            {
                return;
            }

            foreach (PathNode node in job1)
            {
                Delete(node.ID);
            }
        }
    }
}