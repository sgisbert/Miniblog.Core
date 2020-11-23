using Model.DTO.Projects;

using System.Collections.Generic;

namespace Shared.Services
{
    public interface IBusinessService
    {
        List<DtoBusiness> LoadBusiness();
    }
}
