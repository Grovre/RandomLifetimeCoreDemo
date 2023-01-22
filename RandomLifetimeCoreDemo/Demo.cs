using System.Numerics;
using System.Text;

namespace RandomLifetimeCoreDemo;

public class Demo
{
    public static void Main()
    {
        var rand = new Random();
        const int iterations = 1_000;
        var sb = new StringBuilder(72 * iterations);

        void OnSquareChanged(object? sender, SquareEventRequestData data)
        {
            sb.AppendLine(
                $"A square has been changed with a new side length of {data.NewSideLength} and name of {data.NewName}");
        }
        
        for (var i = 0; i < iterations; i++)
        {
            var sq = new Square(rand.Next(0, 10), "before");
            sq.RequestFulfilledEvent += OnSquareChanged;
            var requestEvent = new SquareEventRequestData(sq, "after", rand.Next(10, 20));
            sq.MakeRequest(null, requestEvent);
        }

        Console.WriteLine(sb);
    }
}

public class Square : IRequestAndEvent<SquareEventRequestData>
{
    public string Name { get; protected set; }
    public int Sides { get; protected set; }

    public Square(int sides, string name)
    {
        Name = name;
        Sides = sides;
        RequestMadeEvent += ReceiveRequests;
    }

    private static void ReceiveRequests(object? requester, SquareEventRequestData eventArgs)
    {
        var sq = eventArgs.Sq;
        if (eventArgs.NewName != null)
        {
            sq.Name = eventArgs.NewName;
        }
        if (eventArgs.NewSideLength.HasValue)
        {
            sq.Sides = eventArgs.NewSideLength.Value;
        }
        sq.MakeEvent(sq, new SquareEventRequestData(sq, sq.Name, sq.Sides));
    }

    public event EventHandler<SquareEventRequestData>? RequestFulfilledEvent;
    public event EventHandler<SquareEventRequestData>? RequestMadeEvent;
    public void MakeRequest(object? requester, SquareEventRequestData eventArgs)
    {
        RequestMadeEvent?.Invoke(requester, eventArgs);
    }

    public void MakeEvent(object? fulfiller, SquareEventRequestData eventArgs)
    {
        RequestFulfilledEvent?.Invoke(fulfiller, eventArgs);
    }
}

public abstract class UniqueEvent
{
    public readonly Guid UniqueEventId;

    public UniqueEvent()
    {
        UniqueEventId = Guid.NewGuid();
    }
}

public class SquareEventRequestData : UniqueEvent
{
    public readonly string? NewName;

    public readonly int? NewSideLength;

    public readonly Square Sq;

    public SquareEventRequestData(Square square, string? newName, int? newSideLength)
    {
        Sq = square;
        NewName = newName;
        NewSideLength = newSideLength;
    }
}