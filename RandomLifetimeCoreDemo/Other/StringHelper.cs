using System.Runtime.InteropServices;
using System.Text;

namespace RandomLifetimeCoreDemo.Other;

public static class StringHelper
{
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