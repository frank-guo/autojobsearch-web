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

        public Job2PathService(IRepository<Job2Position> repository)
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