using System.Collections.Concurrent;

namespace RandomLifetimeCoreDemo.Other;

public sealed class ObjectPool<T>
{
    private ConcurrentBag<T> _bag;
    private Func<T> _gen;

    public ObjectPool(Func<T> generator, int initialIdleCount = 0)
    {
        _gen = generator;
        for (var i = 0; i < initialIdleCount; i++)
        {
            _bag.Add(_gen());
        }
    }

    public T Rent()
    {
        var retrieved = _bag.TryTake(out var o);
        if (!retrieved || o == null)
        {
            return _gen();
        }

        return o;
    }

    public void Return(T o)
    {
        _bag.Add(o);
    }
}