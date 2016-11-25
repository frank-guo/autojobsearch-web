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

        public OthersPathService(IRepository<Others> repository)
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