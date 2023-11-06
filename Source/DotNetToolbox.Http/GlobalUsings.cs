// Global using directives

global using System.Diagnostics.CodeAnalysis;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net.Http.Headers;
global using System.Results;
global using System.Security.Claims;
global using System.Text;
global using System.Validation;

global using DotNetToolbox.Http.Options;

global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Options;
global using Microsoft.Identity.Client;
global using Microsoft.IdentityModel.Tokens;

global using static System.Constants.Messages;
global using static System.Ensure;
global using static System.Results.Result;

global using static DotNetToolbox.Http.AuthenticationScheme;
global using static DotNetToolbox.Http.AuthenticationType;

global using Result = System.Results.Result;
