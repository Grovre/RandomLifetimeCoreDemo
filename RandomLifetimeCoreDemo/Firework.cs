using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other;

namespace RandomLifetimeCoreDemo;

public class Firework : LivingInstance, IUnique
{
    public Guid UniqueIdentifier { get; set; }
    private Random _random;
    public Firework(Random random, long birthTime, long plannedDeathTime) : base(birthTime, plannedDeathTime)
    {
        this.RefreshUniqueId();
        _random = random;
    }

    public static Firework Random(Random random, TimeSpan maxDeathTime)
    {
        var randomDeathTime = TimeHelper.RandomDateTimeFromNowWithin(maxDeathTime, random);
        var fw = new Firework(random, DateTime.Now.Ticks, randomDeathTime.Ticks);
        return fw;
    }
    
    internal override void WhenPastExpectedDeath()
    {
        var ohs = new string('O', System.Random.Shared.Next(2, 6));
        Console.WriteLine($"B{ohs}M!");
    }
}