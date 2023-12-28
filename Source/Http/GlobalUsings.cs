// Global using directives

global using System.Diagnostics.CodeAnalysis;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net.Http.Headers;
global using System.Security.Claims;
global using System.Text;

global using DotNetToolbox.Http.Options;
global using DotNetToolbox.Options;
global using DotNetToolbox.Validation;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Options;
global using Microsoft.Identity.Client;
global using Microsoft.IdentityModel.Tokens;

global using static DotNetToolbox.Constants.Messages;
global using static DotNetToolbox.Ensure;
global using static DotNetToolbox.Http.AuthenticationScheme;
global using static DotNetToolbox.Http.AuthenticationType;
global using static DotNetToolbox.Results.Result;

global using Result = DotNetToolbox.Results.Result;
