using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Services
{
    public class Job2PathService : IJob2PathService
    {
        private IRepository<Job2Position> repository;
        private IRepository<RecruitingSite> recruitingSiteRepository;

        public Job2PathService(IRepository<Job2Position> repository,
            IRepository<RecruitingSite> recruitingSiteRepository)
        {
            this.repository = repository;
            this.recruitingSiteRepository = recruitingSiteRepository;
        }

        public IList<Job2Position> Get(int siteId)
        {
            if (siteId <= 0)
            {
                return null;
            }
            RecruitingSite site = recruitingSiteRepository.GetByID(siteId);

            return site.Job2Path;
        }

        public void Update(IList<int> job2Path, int id)
        {
            if (job2Path == null || id <= 0)
            {
                return;
            }

            RecruitingSite site = recruitingSiteRepository.GetByID(id);
            IList<Job2Position> job2Positions = site.Job2Path;
            Delete(job2Positions);

            foreach (int position in job2Path)
            {
                Job2Position job2Position = new Job2Position();
                job2Position.position = position;
                job2Positions.Add(job2Position);
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

        public void Delete(IList<Job2Position> job2)
        {
            if (job2 == null || job2.Count == 0)
            {
                return;
            }

            foreach (Job2Position position in job2)
            {
                Delete(position.ID);
            }
        }
    }
}