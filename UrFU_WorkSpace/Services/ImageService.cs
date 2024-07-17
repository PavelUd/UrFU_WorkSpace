using System.Net.Mime;
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

    public IEnumerable<Image> ConstructImages(List<string> urls, int idOwner = 0, int id = 0)
    {
        var images = new List<Image>();
        foreach (var url in urls)
        {
            var image = new Image()
            {
                Id = id,
                Url = url,
                IdOwner = idOwner
            };
            images.Add(image);
        }

        return images;
    }
    
    public bool CreateImages(int idOwner, List<Image> images)
    {
        foreach (var image in images)
        {
            if (!Repository.CreateImage(image))
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