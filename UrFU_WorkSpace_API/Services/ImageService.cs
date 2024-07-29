using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services;

public class ImageService
{
    private const string ImageDirectoryPath = "images/";

    private readonly IBaseRepository<Image> _repository;
    private readonly string _hostName;

    public ImageService(IBaseRepository<Image> repository, string hostName)
    {
        _repository = repository;
        _hostName = hostName;
    }


    private static void Save(IFormFile imageFile, string path)
    {
        using var fileStream = new FileStream(path.ToUpper(), FileMode.Create);
        imageFile.CopyTo(fileStream);
    }

    public Image ConstructImage(IFormFile imageFile, int idOwner)
    {
        return ConstructImages(new[] { imageFile }, idOwner).First();
    }
    
    public IEnumerable<Image> ConstructImages(IEnumerable<IFormFile> imageFiles, int idOwner)
    {
        var images = new List<Image>();
        foreach (var imageFile in imageFiles)
        {
            var path = ImageDirectoryPath.ToLower() + imageFile.FileName;
            Save(imageFile, path);
            images.Add(new Image
            {
                Url = _hostName + "/api/" + path,
                IdOwner = idOwner
            });
        }

        return images;
    }
}