namespace RandomLifetimeCoreDemo.Other;

/// <summary>
/// This interface provides a simple way
/// to associate a GUID with an instance
/// of an implementing class.
/// </summary>
public interface IUnique
{
    /// <summary>
    /// The GUID an instance of IUnique is associated with.
    /// </summary>
    public Guid UniqueIdentifier { get; internal set; }
}

/// <summary>
/// A static class that provides
/// a function to generate a new GUID
/// to an instance of a class implementing
/// IUnique.
/// </summary>
public static class UniqueHelper
{
    /// <summary>
    /// Creates and assigns an object to a new GUID. 
    /// Allows for versioning of objects if needed.
    /// </summary>
    /// <param name="o">The object receiving a new GUID</param>
    public static void RefreshUniqueId(this IUnique o)
    {
        o.UniqueIdentifier = Guid.NewGuid();
    }
}