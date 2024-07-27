namespace UrFU_WorkSpace_API.Dto;

public class JWTToken
{
    public string TokenType = "Bearer";

    public JWTToken(string accessToken)
    {
        AccessToken = accessToken;
    }

    public string AccessToken { get; set; }
}