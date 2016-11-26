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

        public void Save(Email email)
        {
            var eml = repository.GetByID(email.ID);
            if (eml == null)
            {
                repository.Insert(email);
            }
            else
            {
                eml.password = email.password;
                eml.sendingOn = email.sendingOn;
                eml.sendingTime = email.sendingTime;
                eml.smtpAddress = email.smtpAddress;
                eml.smtpPort = email.smtpPort;
                eml.address = eml.address;
                eml.frequency = email.frequency;
                repository.Update(eml);
            }
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