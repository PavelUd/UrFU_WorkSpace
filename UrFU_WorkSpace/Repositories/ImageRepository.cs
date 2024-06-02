using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class ImageRepository : IImageRepository
{
    private Uri BaseAddress;
    //fgfg
    public ImageRepository(string baseApiAddress)
    {
        BaseAddress = new Uri(baseApiAddress + "/workspaces");
    }
    
    public bool CreateImage(Dictionary<string, object> image)
    {
        var message = HttpRequestSender.SendRequest(BaseAddress + "/add-image", RequestMethod.Put, image).Result;
        return message.IsSuccessStatusCode;
    }
}