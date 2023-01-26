using System.Collections.Concurrent;

namespace RandomLifetimeCoreDemo.Other;

/// <summary>
/// A class similar to ArrayPool and MemoryPool that
/// holds references to instances of T.
///
/// When rented from, this class will no longer have
/// any attachment to the returned object. If there
/// are no available objects in the pool, it will
/// use the generator function to return the user
/// a new object.
///
/// This class is composed with a ConcurrentBag that
/// allows thread-safe usage.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ObjectPool<T>
{
    private readonly ConcurrentBag<T> _bag;
    private readonly Func<T> _gen;

    /// <summary>
    /// Creates an instance of the object pool.
    /// </summary>
    /// <param name="generator">The function to create initial objects and when there aren't enough instances in the pool.</param>
    /// <param name="initialIdleCount">How many objects of T to open the pool with ready to be rented.</param>
    public ObjectPool(Func<T> generator, int initialIdleCount = 0)
    {
        _bag = new();
        _gen = generator;
        
        for (var i = 0; i < initialIdleCount; i++)
        {
            _bag.Add(_gen());
        }
    }

    /// <summary>
    /// Rents an object from the pool. If
    /// there are no objects in the pool, a
    /// new object will be created with the
    /// generator function.
    ///
    /// Any objects returned by this function
    /// will no longer be referenced by this
    /// object.
    /// </summary>
    /// <returns>An existing object, or new if none exist</returns>
    public T Rent()
    {
        var retrieved = _bag.TryTake(out var o);
        if (!retrieved || o == null)
        {
            return _gen();
        }

        return o;
    }

    /// <summary>
    /// Places the passed object back into
    /// the pool.
    /// </summary>
    /// <param name="o">The object going into the pool</param>
    public void Return(T o)
    {
        _bag.Add(o);
    }
}