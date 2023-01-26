namespace RandomLifetimeCoreDemo.Other.Helpers;

public static class TimeHelper
{
    /// <summary>
    /// Generates a random time within DateTime.Now and
    /// the TimeSpan provided by generating a random
    /// long between 0 and total milliseconds of the
    /// given TimeSpan and adding it to DateTime.Now.
    /// </summary>
    /// <param name="within">The TimeSpan for a random time to be generated within from now</param>
    /// <param name="random">The random object used to generate a long between 0 and within.TotalMilliseconds</param>
    /// <returns>A random time within the given TimeSpan and DateTime.Now</returns>
    public static DateTime RandomDateTimeFromNowWithin(TimeSpan within, Random random)
    {
        var randomMsIntoFuture = random.NextInt64((long)within.TotalMilliseconds);
        return DateTime.Now.AddMilliseconds(randomMsIntoFuture);
    }
    
    /// <summary>
    /// See RandomDateTimeFromNowWithin. This
    /// function uses DateTime.UtcNow instead
    /// of DateTime.Now.
    /// </summary>
    /// <param name="within"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public static DateTime RandomDateTimeFromUtcNowWithin(TimeSpan within, Random random)
    {
        var randomMsIntoFuture = random.NextInt64((long)within.TotalMilliseconds);
        return DateTime.UtcNow.AddMilliseconds(randomMsIntoFuture);
    }
}