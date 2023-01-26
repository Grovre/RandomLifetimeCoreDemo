namespace RandomLifetimeCoreDemo.Other.Helpers;

public static class TimeHelper
{
    public static DateTime RandomDateTimeFromNowWithin(TimeSpan within, Random random)
    {
        var min = DateTime.Now;
        var max = min + within;

        var randomTime = new DateTime(random.NextInt64(min.Ticks, max.Ticks));
        return randomTime;
    }
    
    public static DateTime RandomDateTimeFromUtcNowWithin(TimeSpan within, Random random)
    {
        var min = DateTime.UtcNow;
        var max = min + within;

        var randomTime = new DateTime(random.NextInt64(min.Ticks, max.Ticks));
        return randomTime;
    }
}