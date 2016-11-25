using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Services
{
    public class OthersPathService : IOthersPathService
    {
        private IRepository<Others> repository;
        private IRepository<RecruitingSite> recruitingSiteRepository;

        public OthersPathService(IRepository<Others> repository,
            IRepository<RecruitingSite> recruitingSiteRepository)
        {
            this.repository = repository;
            this.recruitingSiteRepository = recruitingSiteRepository;
        }

        public IList<Others> Get(int siteId)
        {
            if (siteId <= 0)
            {
                return null;
            }

            RecruitingSite site = recruitingSiteRepository.GetByID(siteId);
            return site.othersPath;
        }

        public void Update(IList<int> othersPath, int id)
        {
            if (othersPath == null || id <= 0)
            {
                return;
            }

            RecruitingSite site = recruitingSiteRepository.GetByID(id);
            IList<Others> othersPositions = site.othersPath;
            Delete(othersPositions);

            foreach (int position in othersPath)
            {
                Others others = new Others();
                others.position = position;
                othersPositions.Add(others);
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

        public void Delete(IList<Others> othersPath)
        {
            if (othersPath == null || othersPath.Count == 0)
            {
                return;
            }

            foreach (Others company in othersPath)
            {
                Delete(company.ID);
            }
        }
    }
}