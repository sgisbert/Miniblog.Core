using Model.DTO.Projects;

using System.Collections.Generic;

namespace Shared.Services
{
    public interface IImagesService
    {
        List<DtoImage> LoadImages();
        string GetUrl(DtoImage image);
        string GetUrlThumbnail(DtoImage image);
    }
}
