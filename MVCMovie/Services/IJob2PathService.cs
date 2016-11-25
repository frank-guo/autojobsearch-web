﻿using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCMovie.Services
{
    public interface IJob2PathService
    {
        void Delete(int id);
        void Delete(IList<Job2Position> job1);
    }
}
