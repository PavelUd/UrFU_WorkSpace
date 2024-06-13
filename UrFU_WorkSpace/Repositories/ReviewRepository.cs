using Newtonsoft.Json;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services;

public class ReviewRepository
{
    private readonly Uri BaseAddress;

    public ReviewRepository(string baseAddress)
    {
        BaseAddress = new Uri(baseAddress);
    }
    
    public List<Review> GetAll()
    {
        var message = HttpRequestSender.SendRequest(BaseAddress + "/reviews", RequestMethod.Get)
            .Result.Content
            .ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<IEnumerable<Review>>(message)!.ToList() ?? new List<Review>();
    }

    public Review? GetById(int id)
    {
        return GetAll().Find(r => r.Id == id);
    }
    
    public IEnumerable<Review> GetByIdWorkspace(int id)
    {
        return GetAll().Where(r => r.IdWorkspace == id);
    }
    
    public IEnumerable<Review> GetByIdUser(int id)
    {
        return GetAll().Where(r => r.IdUser == id);
    }
    
    public void AddReview(Review review)
    {
        var message = HttpRequestSender.SendRequest(BaseAddress + "/reviews/add-review", RequestMethod.Post, review);
    }
}