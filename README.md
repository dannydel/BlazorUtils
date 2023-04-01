# BlazorUtils
A few different code snippets that can eventually become a project of .NET Blazor Utilities.

## StateService.cs
Here's how you can use it in your components:

### Register the StateService in your DI container.
```C#
services.AddSingleton<StateService>();
```

Inject the StateService into your components.
```C#
@inject StateService State
Use the StateService to get and set state, and subscribe to changes.
```
#### Set state
```C#
State.Set("myKey", "myValue");
```

#### Get state
```C#
string value = State.Get<string>("myKey");
```

#### Subscribe to changes
```C#
State.Subscribe<string>("myKey", newValue => {
    Console.WriteLine($"New value: {newValue}");
});
```




