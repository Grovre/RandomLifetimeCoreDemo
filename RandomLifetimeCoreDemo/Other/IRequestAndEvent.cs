namespace RandomLifetimeCoreDemo;

public interface IRequestAndEvent<TEventArgs>
{
    public event EventHandler<TEventArgs>? RequestFulfilledEvent;
    public event EventHandler<TEventArgs>? RequestMadeEvent;
    
    public void MakeRequest(object? requester, TEventArgs eventArgs);
    public void MakeEvent(object? fulfiller, TEventArgs eventArgs);
}

public interface IRequestAndEvent<TRequest, TEvent>
{
    public event EventHandler<TEvent>? RequestFulfilledEvent;
    public event EventHandler<TRequest>? RequestMadeEvent;

    public void MakeRequest(object? requester, TRequest request);
    public void MakeEvent(object? fulfiller, TEvent result);
}