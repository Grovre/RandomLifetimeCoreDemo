namespace RandomLifetimeCoreDemo.Other;

/// <summary>
/// This interface provides a simple way to
/// make implementations of the CORE design
/// pattern simpler.
///
/// Generally, this class will be
/// implemented on modules of the CORE
/// design pattern and all requests should be
/// followed up by an event.
/// </summary>
/// <typeparam name="TEventArgs">The event and request args object type</typeparam>
public interface IRequestAndEvent<TEventArgs>
{
    public event EventHandler<TEventArgs>? RequestFulfilledEvent;
    public event EventHandler<TEventArgs>? RequestMadeEvent;
    
    public void MakeRequest(object? requester, TEventArgs eventArgs);
    public void MakeEvent(object? fulfiller, TEventArgs eventArgs);
}

/// <summary>
/// See IRequestAndEvent<TEventArgs> for more
/// information. This interface involves two
/// generic types instead of one to differentiate
/// between request objects and response objects.
/// </summary>
/// <typeparam name="TRequest">The request object type</typeparam>
/// <typeparam name="TEvent">The event object type</typeparam>
public interface IRequestAndEvent<TRequest, TEvent>
{
    public event EventHandler<TEvent>? RequestFulfilledEvent;
    public event EventHandler<TRequest>? RequestMadeEvent;

    public void MakeRequest(object? requester, TRequest request);
    public void MakeEvent(object? fulfiller, TEvent result);
}