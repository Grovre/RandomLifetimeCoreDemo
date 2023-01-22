namespace RandomLifetimeCoreDemo;

public interface IUnique
{
    public Guid UniqueIdentifier { get; internal set; }
}

public static class UniqueHelper
{
    public static void RefreshUniqueId(this IUnique o)
    {
        o.UniqueIdentifier = Guid.NewGuid();
    }
}