namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal class TestInput(params string[] inputs) : IInput {
    private readonly Queue<string> _inputQueue = new(inputs);

    public bool KeyAvailable() => throw new NotImplementedException();

    public int Read() => throw new NotImplementedException();

    public ConsoleKeyInfo ReadKey(bool intercept = false) => throw new NotImplementedException();

    public string? ReadLine() => _inputQueue.TryDequeue(out var input) ? input : null;

    public Encoding Encoding {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public TextReader Reader => throw new NotImplementedException();
}
