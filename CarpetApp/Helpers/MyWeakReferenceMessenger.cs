namespace CarpetApp.Helpers;

public class MyWeakReferenceMessenger
{
    private readonly Dictionary<string, WeakReference<object>> _subscribers = new Dictionary<string, WeakReference<object>>();

    public void Subscribe<T>(string message, Action<T> callback)
    {
        lock (_subscribers)
        {
            if (_subscribers.TryGetValue(message, out var weakReference))
            {
                if (weakReference.TryGetTarget(out var subscriber))
                {
                    if (subscriber is T typedSubscriber)
                    {
                        _subscribers[message] = new WeakReference<object>(typedSubscriber);
                    }
                }
            }
            else
            {
                _subscribers[message] = new WeakReference<object>(callback);
            }
        }
    }

    public void Unsubscribe(string message)
    {
        lock (_subscribers)
        {
            _subscribers.Remove(message);
        }
    }

    public void Send<T>(string message, T value)
    {
        lock (_subscribers)
        {
            if (_subscribers.TryGetValue(message, out var weakReference))
            {
                if (weakReference.TryGetTarget(out var subscriber))
                {
                    if (subscriber is Action<T> typedSubscriber)
                    {
                        typedSubscriber(value);
                    }
                }
            }
        }
    }
}