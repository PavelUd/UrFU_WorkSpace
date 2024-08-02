using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services;

public class ImageService
{
    private readonly string imageDirectoryPath;

    private readonly IBaseRepository<Image> _repository;
    private readonly string _hostName;

    public ImageService(IBaseRepository<Image> repository, string hostName, string path)
    {
        imageDirectoryPath = path;
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
            var path = imageDirectoryPath.ToLower() + imageFile.FileName;
            Save(imageFile, path);
            images.Add(new Image
            {
                Url = _hostName  + path,
                IdOwner = idOwner
            });
        }

        return images;
    }
}