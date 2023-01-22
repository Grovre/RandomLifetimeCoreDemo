using RandomLifetimeCoreDemo.Living.LifetimeReactions.Death;

namespace RandomLifetimeCoreDemo.Living;

public abstract class LivingInstance
{
    public readonly long BirthTime;
    public readonly long PlannedDeathTime;
    public bool RemoveFromWatchAfterDeathTime = true;

    protected LivingInstance(long birthTime, long plannedDeathTime)
    {
        BirthTime = birthTime;
        PlannedDeathTime = plannedDeathTime;
    }

    internal abstract void WhenPastExpectedDeath();
}