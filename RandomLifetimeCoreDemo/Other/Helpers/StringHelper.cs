namespace RandomLifetimeCoreDemo.Other.Helpers;

public static class StringHelper
{
    /// <summary>
    /// Repeats a string x amount of times by
    /// copying to a stack-allocated char span.
    /// Beware of using this for strings that will
    /// end up being large.
    /// </summary>
    /// <param name="str">The string to be repeated</param>
    /// <param name="times">The amount of times to be repeated</param>
    /// <returns>A string repeated x amount of times</returns>
    public static string RepeatStack(this string str, int times)
    {
        var strChars = str.AsSpan();
        Span<char> repeatChars = stackalloc char[str.Length * times];
        
        for (var i = 0; i < times; i++)
        {
            var beginIndex = i * strChars.Length;
            strChars.CopyTo(repeatChars[beginIndex..]);
        }
        
        return new string(repeatChars);
    }
    
    /// <summary>
    /// Repeats a string x amount of times by
    /// copying to a heap-allocated span.
    /// Beware of using this for strings that will
    /// end up being large.
    /// </summary>
    /// <param name="str">The string to be repeated</param>
    /// <param name="times">The amount of times to be repeated</param>
    /// <returns>A string repeated x amount of times</returns>
    public static string Repeat(this string str, int times)
    {
        var strChars = str.AsSpan();
        var repeatChars = new char[str.Length * times].AsSpan();
        
        for (var i = 0; i < times; i++)
        {
            var beginIndex = i * strChars.Length;
            strChars.CopyTo(repeatChars[beginIndex..]);
        }
        
        return new string(repeatChars);
    }
}