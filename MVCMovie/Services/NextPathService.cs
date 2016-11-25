using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Services
{
    public class NextPathService : INextPathService
    {
        private IRepository<NextPosition> repository;
        private IRepository<RecruitingSite> recruitingSiteRepository;

        public NextPathService(IRepository<NextPosition> repository,
            IRepository<RecruitingSite> recruitingSiteRepository)
        {
            this.repository = repository;
            this.recruitingSiteRepository = recruitingSiteRepository;
        }

        public void Update(NextPosition nextPosition)
        {
            if (nextPosition == null)
            {
                return;
            }
            repository.Update(nextPosition);
        }

        public void Update(IList<int> nextPath, int siteId)
        {
            if (nextPath == null)
            {
                return;
            }

            RecruitingSite site = recruitingSiteRepository.GetByID(siteId);
            IList<NextPosition> nextPositions = site.ListNextPositions;
            Delete(nextPositions);
            
            foreach(int position in nextPath) 
            {
                NextPosition np = new NextPosition();
                np.position = position;
                nextPositions.Add(np);
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

        public void Delete(IList<NextPosition> nextPath)
        {
            if (nextPath == null || nextPath.Count == 0)
            {
                return;
            }

            foreach (NextPosition nextPosition in nextPath)
            {
                Delete(nextPosition.ID);
            }
        }
    }
}