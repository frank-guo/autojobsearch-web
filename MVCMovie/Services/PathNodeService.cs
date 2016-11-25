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
        private IRepository<RecruitingSite> recruitingSiteRepository;

        public PathNodeService(IRepository<PathNode> repository,
            IRepository<RecruitingSite> recruitingSiteRepository)
        {
            this.repository = repository;
            this.recruitingSiteRepository = recruitingSiteRepository;
        }

        public IList<PathNode> Get(int siteId)
        {
            if (siteId <= 0)
            {
                return null;
            }

            RecruitingSite site = recruitingSiteRepository.GetByID(siteId);
            return site.JobPath;
        }

        public void Update(IList<PathNode> job1Path, int id)
        {
            if (job1Path == null || id <= 0)
            {
                return;
            }

            RecruitingSite site = recruitingSiteRepository.GetByID(id);
            IList<PathNode> job1Nodes = site.JobPath;
            Delete(job1Nodes);

            foreach (PathNode node in job1Path)
            {
                job1Nodes.Add(node);
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