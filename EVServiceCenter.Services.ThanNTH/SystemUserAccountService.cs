using EVServiceCenter.Repositories.ThanNTH;
using EVServiceCenter.Repositories.ThanNTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public class SystemUserAccountService
{
    private readonly SystemUserAccountRepository _repository;
    public SystemUserAccountService()
    {
        _repository = new SystemUserAccountRepository();
    }

    public async Task<SystemUserAccount> GetUserAccount(string userName, string password)
    {
        try
        {
            return await _repository.GetUserAccount(userName, password);
        }
        catch (Exception ex)
        {

        }
        return null;
    }
}
