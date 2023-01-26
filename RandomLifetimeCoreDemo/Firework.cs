using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other;
using RandomLifetimeCoreDemo.Other.Helpers;

namespace RandomLifetimeCoreDemo;

public class Firework : LivingInstance, IUnique
{
    public Guid UniqueIdentifier { get; set; }

    public Firework(DateTime plannedDeathTime) : base(plannedDeathTime)
    {
        this.RefreshUniqueId();
    }

    public static Firework Random(Random random, TimeSpan maxDeathTime)
    {
        var randomDeathTime = TimeHelper.RandomDateTimeFromNowWithin(maxDeathTime, random);
        var fw = new Firework(randomDeathTime);
        fw.OnDeath += () => Console.WriteLine($"B{"O".RepeatStack(random.Next(2, 6))}M!");
        return fw;
    }
}