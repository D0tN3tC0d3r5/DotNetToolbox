global using System.Diagnostics.CodeAnalysis;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;

global using DotNetToolbox.Results;

global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

global using OpenTelemetry;
global using OpenTelemetry.Metrics;
global using OpenTelemetry.Trace;

global using WebApi.Builders;
global using WebApi.Contracts.Authentication;
global using WebApi.Model;
global using WebApi.Options;
global using WebApi.Services;
global using WebApi.Tokens;
global using WebApi.Utilities;
