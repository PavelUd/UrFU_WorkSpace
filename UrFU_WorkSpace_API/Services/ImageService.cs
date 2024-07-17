using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services;

public class ImageService
{
    private string HostName = "hi";
    private const string ImageDirectoryPath = "images/";

    private readonly IBaseRepository<Image> Repository;
    public ImageService(IBaseRepository<Image> repository)
    {
        Repository = repository;
    }
    
    
    private static void Save(IFormFile imageFile, string path)
    {
        using var fileStream = new FileStream(path.ToUpper(), FileMode.Create);
        imageFile.CopyTo(fileStream);
    }

    public IEnumerable<Image> ConstructImages(IEnumerable<IFormFile> imageFiles, int idOwner = 0)
    {
        var images = new List<Image>();
        foreach (var imageFile in imageFiles)
        {
            var path = ImageDirectoryPath.ToLower() + imageFile.FileName;
            Save(imageFile, path);
            images.Add(new Image
            {
                Url = HostName + "/api/" + path,
                IdOwner = idOwner,
            });
        }

        return images;
    }
}