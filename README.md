# BlazorUtils
A few different code snippets that can eventually become a project of .NET Blazor Utilities.

## StateService.cs
Here's how you can use it in your components:

### Register the StateService in your DI container.
*This is project dependent
```C# - Blazor WASM
services.AddSingleton<StateService>();
```

```C# Blazor - Service
services.AddScoped<StateServiec>();
```

We want to be explicit here and do not want to mistakenly give Blazor Server a singleton since that would make it available for all users in the app. Since WASM gets downloaded per client adding a singleton makes sense.

Inject the StateService into your components.
```C#
@inject StateService State
```
Use the StateService to get and set state, and subscribe to changes.

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


# Reflective Components
Refelective components utilize the data models that we pass into.




