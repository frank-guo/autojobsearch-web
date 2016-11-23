using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCMovie.Repositories;
using System.Linq.Expressions;
using MVCMovie.Models;

namespace MVCMovie.Services
{
    public interface IRecruitingSiteService
    {
        IEnumerable<WebsiteViewModel> Get(Expression<Func<RecruitingSite, bool>> filter = null, string includeProperties = "");
        RecruitingSite GetByID(int id);
        void CreateSite(string url, string siteName);
        void Delete(int id);
        void Update(RecruitingSite site);
        void Update(int id, string url, string siteName);
    }
}