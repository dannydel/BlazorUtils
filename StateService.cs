public class StateService
{
    private Dictionary<string, object> state = new Dictionary<string, object>();
    private Dictionary<string, Action<object>> subscribers = new Dictionary<string, Action<object>>();

    public T Get<T>(string key)
    {
        if (state.TryGetValue(key, out object value))
        {
            return (T)value;
        }
        return default;
    }

    public void Set<T>(string key, T value)
    {
        state[key] = value;
        if (subscribers.TryGetValue(key, out Action<object> callback))
        {
            callback.Invoke(value);
        }
    }

    public void Subscribe<T>(string key, Action<T> callback)
    {
        if (!subscribers.ContainsKey(key))
        {
            subscribers.Add(key, obj => callback.Invoke((T)obj));
        }
        else
        {
            subscribers[key] += obj => callback.Invoke((T)obj);
        }
    }

    public void Unsubscribe<T>(string key, Action<T> callback)
    {
        if (subscribers.TryGetValue(key, out Action<object> existingCallback))
        {
            subscribers[key] = obj => existingCallback(obj);
            subscribers[key] -= obj => callback.Invoke((T)obj);
        }
    }
}
