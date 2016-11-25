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

        public NextPathService(IRepository<NextPosition> repository)
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