using RandomLifetimeCoreDemo.Living.LifetimeReactions.Death;

namespace RandomLifetimeCoreDemo.Living;

public abstract class LivingInstance
{
    public readonly DateTime BirthTime;
    public readonly DateTime PlannedDeathTime;

    protected LivingInstance(DateTime birthTime, DateTime plannedDeathTime)
    {
        BirthTime = birthTime;
        PlannedDeathTime = plannedDeathTime;
    }

    internal abstract void WhenPastExpectedDeath();
}