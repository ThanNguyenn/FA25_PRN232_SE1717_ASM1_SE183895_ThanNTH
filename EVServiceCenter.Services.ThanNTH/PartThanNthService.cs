using EVServiceCenter.Repositories.ThanNTH;
using EVServiceCenter.Repositories.ThanNTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public class PartThanNthService
{
    private readonly PartThanNthRepository _repository;

    public PartThanNthService()
    {
        _repository = new PartThanNthRepository();
    }

    public async Task<List<PartThanNth>> GetAllPartsAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
        
        }
        return new List<PartThanNth>();
    }
}
