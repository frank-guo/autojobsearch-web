﻿using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCMovie.Services
{
    public interface IEmailSettingService
    {
        void Save(Email email);
        Email GetById(int siteId);
    }
}
