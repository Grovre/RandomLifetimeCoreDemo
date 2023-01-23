using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other;

namespace RandomLifetimeCoreDemo;

public class Firework : LivingInstance, IUnique
{
    public Guid UniqueIdentifier { get; set; }

    public Firework(DateTime birthTime, DateTime plannedDeathTime) : base(birthTime, plannedDeathTime)
    {
        this.RefreshUniqueId();
    }

    public static Firework Random(Random random, TimeSpan maxDeathTime)
    {
        var randomDeathTime = TimeHelper.RandomDateTimeFromNowWithin(maxDeathTime, random);
        var fw = new Firework(DateTime.Now, randomDeathTime);
        return fw;
    }
    
    internal override void WhenPastExpectedDeath()
    {
        var ohs = new string('O', System.Random.Shared.Next(2, 6));
        Console.WriteLine($"B{ohs}M!");
    }
}