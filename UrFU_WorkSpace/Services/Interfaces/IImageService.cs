namespace UrFU_WorkSpace.Services.Interfaces;

public interface IImageService
{
    public bool CreateImages(int idOwner, List<string> urls);
    
    public List<string> SaveImages(IWebHostEnvironment appEnvironment, IFormFileCollection images);
}