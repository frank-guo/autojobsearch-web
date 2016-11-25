using MVCMovie.Models;
using MVCMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Services
{
    public class CompanyPathService : ICompanyPathService
    {
        private IRepository<Company> repository;
        private IRepository<RecruitingSite> recruitingSiteRepository;

        public CompanyPathService(IRepository<Company> repository,
            IRepository<RecruitingSite> recruitingSiteRepository)
        {
            this.repository = repository;
            this.recruitingSiteRepository = recruitingSiteRepository;
        }

        public void Update(IList<int> companyPath, int id)
        {
            if (companyPath == null || id <= 0)
            {
                return;
            }

            RecruitingSite site = recruitingSiteRepository.GetByID(id);
            IList<Company> companyPositions = site.companyPath;
            Delete(companyPositions);

            foreach (int position in companyPath)
            {
                Company company = new Company();
                company.position = position;
                companyPositions.Add(company);
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

        public void Delete(IList<Company> companyPath)
        {
            if (companyPath == null || companyPath.Count == 0)
            {
                return;
            }

            foreach (Company company in companyPath)
            {
                Delete(company.ID);
            }
        }
    }
}