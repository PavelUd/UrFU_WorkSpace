using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public static class Utils
{
    public static int GetConfirmCode()
    {
        var random = new Random();
        return random.Next(100000, 1000000);
    }

    public static double GetDistance(Coordinate workspacePos, Coordinate userPos)
    {
        const double r = 6371e3;
        var φ1 = workspacePos.Latitude * Math.PI / 180;
        var φ2 = userPos.Latitude * Math.PI / 180;
        var Δφ = (userPos.Latitude - workspacePos.Latitude) * Math.PI / 180;
        var Δλ = (userPos.Longitude - workspacePos.Longitude) * Math.PI / 180;

        var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                Math.Cos(φ1) * Math.Cos(φ2) *
                Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return r * c;
    }
}