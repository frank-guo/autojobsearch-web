using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Services
{
    public class EmailSettingService : IEmailSettingService
    {
        private IRepository<Email> repository;

        public EmailSettingService(IRepository<Email> repository)
        {
            this.repository = repository;
        }

        public Email GetById(int siteId)
        {
            if (siteId <= 0)
            {
                return null;
            }
            return repository.GetByID(siteId);
        }
    }
}