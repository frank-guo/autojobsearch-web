using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;

namespace MVCMovie.Services
{
    public class RecruitingSiteService : IRecruitingSiteService
    {
        private IRepository<RecruitingSite> repository;

        private ApplicationDbContext ApplicationDbContext;
        private UserManager<ApplicationUser> UserManager;

        public RecruitingSiteService(IRepository<RecruitingSite> repository)
        {
            this.repository = repository;
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public IEnumerable<WebsiteViewModel> Get(Expression<Func<RecruitingSite, bool>> filter = null, string includeProperties = "")
        {
            var name = System.Web.HttpContext.Current.User.Identity.Name;
            var user = this.ApplicationDbContext.Users.FirstOrDefault(u => u.UserName == name);
            var userid = user.Id;

            IEnumerable<RecruitingSite> sites = repository.Get(u => u.ApplicationUser.Id == userid);
            List<WebsiteViewModel> sitesVM = new List<WebsiteViewModel>();
            foreach (RecruitingSite site in sites)
            {
                WebsiteViewModel siteVM = new WebsiteViewModel();
                siteVM.ID = site.ID;
                siteVM.siteName = site.siteName;
                siteVM.url = site.url;
                sitesVM.Add(siteVM);
            }
            return sitesVM;
        }

        public RecruitingSite GetByID(int id)
        {
            if (id < 0) {
                return null;
            }

            return repository.GetByID(id);
        }

        public void CreateSite(string url, string siteName)
        {
            if (url == null || url == "")
            {
                return;
            }
            //RecruitingSite site = new RecruitingSite();
            //site.siteName = siteName;
            //site.url = url;
            //var name = System.Web.HttpContext.Current.User.Identity.Name;
            //var user = this.ApplicationDbContext.Users.FirstOrDefault(u => u.UserName == name);
            ////var user = UserManager.FindById(userId);
            //site.ApplicationUser = user;
            //site.ApplicationUser_Id = user.Id;

            RecruitingSite site = new RecruitingSite();
            site.siteName = siteName;
            site.url = url;
            var name = System.Web.HttpContext.Current.User.Identity.Name;
            var user = this.ApplicationDbContext.Users.FirstOrDefault(u => u.UserName == name);
            user.RecruitingSites.Add(site);
            UserManager.Update(user);

            //repository.Insert(site);
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                return;
            }
            repository.Delete(id);
        }

        public void Update(RecruitingSite site)
        {
            if (site == null)
            {
                return;
            }
            repository.Update(site);
        }

        public void Update(int id, string url, string siteName)
        {
            if (id <= 0 || url == null || url == "")
            {
                return;
            } 

            RecruitingSite site = new RecruitingSite();
            site.ID = id;
            site.url = url;
            site.siteName = siteName;
            repository.Update(site);
        }
    }
}