using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other;
using RandomLifetimeCoreDemo.Other.Helpers;

namespace RandomLifetimeCoreDemo;

/// <summary>
/// A class that shows how LivingInstance
/// can be implemented.
/// </summary>
public class Firework : LivingInstance, IUnique
{
    public Guid UniqueIdentifier { get; set; }

    public Firework(DateTime plannedDeathTime) : base(plannedDeathTime)
    {
        this.RefreshUniqueId();
    }

    /// <summary>
    /// Generates a random Firework object that explodes
    /// randomly within the maxDeathTime from DateTime.Now using
    /// the provided random object.
    /// </summary>
    /// <param name="random">The random object to generate the Firework instance with</param>
    /// <param name="maxDeathTime">The TimeSpan that the random death time will not exceed from DateTime.Now</param>
    /// <returns>A random Firework instance</returns>
    public static Firework Random(Random random, TimeSpan maxDeathTime)
    {
        var randomDeathTime = TimeHelper.RandomDateTimeFromNowWithin(maxDeathTime, random);
        var fw = new Firework(randomDeathTime);
        fw.OnDeath += () => Console.WriteLine($"B{"O".RepeatStack(random.Next(2, 6))}M!");
        return fw;
    }
}