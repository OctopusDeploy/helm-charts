namespace KubernetesAgent.Integration.Setup.Common;

public class PollingSubscriptionIdGenerator
{
    private static Random random = new Random();
    public static Uri Generate()
    {
        return new Uri("poll://" + RandomString(20).ToLowerInvariant() + "/");
    }
    
    static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
