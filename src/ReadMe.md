# Auto Interfaces 
This library is a C# [Source Generator](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) which generates interfaces for concrete classes. Adding `[AutoInterface]` to a class will generate a `I{ClassName}` interface and add it to your project. 


## Features 

### Public Members  
All public  
```cs
public Player 
{
    public string Name { get; }
    public string? Street { get; set; }
    public event Action Jump;

    public void DoSomething()
    {}


    // Ignored
    internal string LastName { get; }
    private string MiddleName { get; }
    protected float Height { get; } 
}

// Generate
public IPlayer 
{
    string Name { get; }
    string? Street { get; set; }
    event Action Jump;
    void DoSomething();
}
```

### Explicit Interface Members
You can define explicit interface members inside that class if it implements it's own generated interface.
```cs
public Player : IPlayer /* Generated */
{
    /// Adds member to interface
    void IPlayer.Kill()
    {

    }
}
```

## TODO:
* [ ] Primary Constructor 
* [ ] Obsolete Attribute
* [ ] Method Parameter Attributes
* [ ] XML Comments 
* [X] Indexers
* [X] Properties 
* [X] Methods 
* [X] Events