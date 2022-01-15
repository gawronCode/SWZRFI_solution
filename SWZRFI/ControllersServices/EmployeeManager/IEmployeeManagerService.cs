﻿using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.ControllersServices.EmployeeManager
{
    public interface IEmployeeManagerService
    {
        Task<bool> SendInvitation(string userEmail, string callBackUrl, CorporationalInvitation invitation);
    }
}
