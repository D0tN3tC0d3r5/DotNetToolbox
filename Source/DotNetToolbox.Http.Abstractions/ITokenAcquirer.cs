namespace DotNetToolbox.Http;

public interface ITokenAcquirer {
    Task<string> AcquireTokenAsync();
}
