using RandomLifetimeCoreDemo.Living.LifetimeReactions.Death;

namespace RandomLifetimeCoreDemo.Living;

public abstract class LivingInstance
{
    public readonly DateTime BirthTime;
    public readonly DateTime PlannedDeathTime;
    public Action? OnDeath;

    protected LivingInstance(DateTime plannedDeathTime)
    {
        BirthTime = DateTime.Now;
        PlannedDeathTime = plannedDeathTime;
    }
}