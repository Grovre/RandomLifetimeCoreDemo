using System.Diagnostics;

namespace RandomLifetimeCoreDemo.Other.Helpers;

public static class RandomHelper
{
    /// <summary>
    /// Generates a random boolean based
    /// off of the weighted fraction,
    /// such as "2 times out of 5" or 2/5.
    /// It's based on true/false ratio.
    /// </summary>
    /// <param name="random">The random object used to generate a bool</param>
    /// <param name="times">The chance a bool will be true out of the next parameter</param>
    /// <param name="outOf">The denominator of the true/false ratio</param>
    /// <returns>A random bool based on the arguments</returns>
    public static bool NextBool(this Random random, int times, int outOf)
    {
        Debug.Assert(times <= outOf);
        return random.Next(outOf) < times;
    }

    /// <summary>
    /// A 50/50 true/false generator.
    /// </summary>
    /// <param name="random">The random object used to generate a bool</param>
    /// <returns>A random bool</returns>
    public static bool NextBool(this Random random)
    {
        return random.Next(0) == 1;
    }
}