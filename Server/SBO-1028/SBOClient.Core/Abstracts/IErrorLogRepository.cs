﻿using SBOClient.Core.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.Abstracts
{
    public interface IErrorLogRepository: IRepository<ErrorLog>
    {
        ErrorLog AddErrLog(ErrorLog err);
    }
}
