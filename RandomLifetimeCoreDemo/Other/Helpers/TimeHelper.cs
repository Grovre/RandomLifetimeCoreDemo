namespace RandomLifetimeCoreDemo.Other.Helpers;

public static class TimeHelper
{
    public static DateTime RandomDateTimeFromNowWithin(TimeSpan within, Random random)
    {
        var randomMsIntoFuture = random.NextInt64((long)within.TotalMilliseconds);
        return DateTime.Now.AddMilliseconds(randomMsIntoFuture);
    }
    
    public static DateTime RandomDateTimeFromUtcNowWithin(TimeSpan within, Random random)
    {
        var randomMsIntoFuture = random.NextInt64((long)within.TotalMilliseconds);
        return DateTime.UtcNow.AddMilliseconds(randomMsIntoFuture);
    }
}