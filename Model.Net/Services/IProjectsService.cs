using Model.DTO.Projects;

using System.Collections.Generic;

namespace Shared.Services
{
    public interface IProjectsService
    {
        List<DtoProject> LoadProjectsFull();

        DtoProject LoadProject(string slug);

        DtoProject LoadProjectData(DtoProject project, List<DtoCategory> categories, List<DtoClient> clients, List<DtoImage> images, List<DtoBusiness> business);

        List<DtoProject> LoadProjects();
    }
}
