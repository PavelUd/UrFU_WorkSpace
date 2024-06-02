using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class ImageService : IImageService
{
    private IImageRepository Repository;
    
    public ImageService(IImageRepository repository)
    {
        Repository = repository;
    }

    public bool CreateImages(int idOwner, List<string> urls)
    {
        foreach (var url in urls)
        {
            var dictionary = new Dictionary<string, object>()
            {
                { "url", url },
                { "idWorkspace", idOwner }
            };
            if (!Repository.CreateImage(dictionary))
            {
                return false;
            };
        }
        return true;
    }

    public List<string> SaveImages(IWebHostEnvironment appEnvironment, IFormFileCollection images)
        {
            var urls = new List<string>();
            foreach(var uploadedFile in images)
            {
                var path = "/Files/" + uploadedFile.FileName;
                using var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create);
                uploadedFile.CopyTo(fileStream);
                urls.Add(path);
            }

            return urls;
        }
}