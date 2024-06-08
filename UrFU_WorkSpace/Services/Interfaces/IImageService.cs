using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IImageService
{
    public bool CreateImages(int idOwner, List<Image> images);
    
    public List<string> SaveImages(IWebHostEnvironment appEnvironment, IFormFileCollection images);

    public IEnumerable<Image> ConstructImages(List<string> urls, int idOwner = 0, int id = 0);
}