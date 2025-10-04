using EVServiceCenter.Repositories.ThanNTH;
using EVServiceCenter.Repositories.ThanNTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public class ServiceCenterHieuPTService
{
    private readonly ServiceCenterHieuPTRepository _repository;

    public ServiceCenterHieuPTService()
    {
        _repository = new ServiceCenterHieuPTRepository();
    }

    public async Task<List<ServiceCenterHieupt>> GetAll()
    {
        return await _repository.GetAll();
    }
}
