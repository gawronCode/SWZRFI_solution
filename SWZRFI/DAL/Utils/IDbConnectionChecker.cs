using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWZRFI.DAL.Utils
{
    public interface IDbConnectionChecker
    {
        Task<bool> ValidateDbConnection(DbContext context);
    }
}
