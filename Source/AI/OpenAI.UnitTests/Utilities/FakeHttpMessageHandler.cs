namespace DotNetToolbox.AI.OpenAI.Utilities;

internal sealed class FakeHttpMessageHandler
    : HttpMessageHandler {
    private Exception? _exception;
    private HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;
    private string _response = string.Empty;

    public void ForceException<TException>(TException exception)
        where TException : Exception
        => _exception = exception;

    public void SetOkResponse<TData>(TData data) {
        _response = Serialize(data);
        _statusCode = HttpStatusCode.OK;
    }

    public void SetNotFoundResponse() {
        _response = string.Empty;
        _statusCode = HttpStatusCode.NotFound;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                           CancellationToken cancellationToken)
        => _exception is not null
               ? Task.FromException<HttpResponseMessage>(_exception)
               : Task.FromResult(new HttpResponseMessage() {
                   StatusCode = _statusCode,
                   Content = new StringContent(_response),
               });
}
