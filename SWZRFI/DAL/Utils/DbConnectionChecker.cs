using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWZRFI.DAL.Utils
{
    public class DbConnectionChecker : IDbConnectionChecker
    {
        public async Task<bool> ValidateDbConnection(DbContext context)
        {
            context.Database.SetCommandTimeout(30);
            return await context.Database.CanConnectAsync();
        }
    }
}
