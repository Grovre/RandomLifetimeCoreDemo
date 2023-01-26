namespace RandomLifetimeCoreDemo.Living;

/// <summary>
/// Base class for objects that have an expected lifetime
/// that includes the time of an instance's birth,
/// an expected death time, and a delegate Action
/// for what happens on the instance's death
/// </summary>
public abstract class LivingInstance
{
    public readonly DateTime BirthTime;
    public readonly DateTime PlannedDeathTime;
    public Action? OnDeath { get; set; }

    protected LivingInstance(DateTime plannedDeathTime)
    {
        BirthTime = DateTime.Now;
        PlannedDeathTime = plannedDeathTime;
    }
}