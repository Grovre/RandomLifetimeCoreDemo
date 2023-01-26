using System.Diagnostics;

namespace RandomLifetimeCoreDemo.Other.Helpers;

public static class RandomHelper
{
    public static bool NextBool(this Random random, int times, int outOf)
    {
        Debug.Assert(times <= outOf);
        return random.Next(outOf) < times;
    }

    public static bool NextBool(this Random random)
    {
        return random.Next(0) == 1;
    }
}