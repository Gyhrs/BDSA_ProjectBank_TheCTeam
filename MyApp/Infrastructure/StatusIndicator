namespace MyApp.Shared;

public struct StatusIndicator<T> where T : class
{
    private readonly T? _value;

    public T Value => _value ?? throw new InvalidOperationException();

    public bool IsNone => _value == null;

    public bool IsSome => _value != null;

    public StatusIndicator(T? value)
    {
        _value = value;
    }

    public static implicit operator T(StatusIndicator<T> option) => option.Value;

    public static implicit operator StatusIndicator<T>(T? value) => new(value);
}
