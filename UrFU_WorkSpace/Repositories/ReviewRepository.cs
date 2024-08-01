using Newtonsoft.Json;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Repositories;

public class ReviewRepository
{
    private readonly Uri BaseAddress;

    public ReviewRepository(string baseAddress)
    {
        BaseAddress = new Uri(baseAddress);
    }
    
    public async Task<Result<List<Review>>> GetAll(int idWorkspace)
    {
        return await HttpRequestSender.HandleJsonRequest<List<Review>>(BaseAddress + $"/reviews?idWorkspace={idWorkspace}", HttpMethod.Get);
    }
    
    public async Task<Result<int>> AddReview(Review review, string token)
    {
       return await HttpRequestSender.HandleJsonRequest<int, Review>(BaseAddress + "/reviews", HttpMethod.Post, review, token);
    }
}