namespace UrFU_WorkSpace_API.Helpers;

public static class Utils
{
    public static int GetConfirmCode()
    {
        var random = new Random();
        return random.Next(100000, 1000000);
    }
}