global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text.Json;

global using DotNetToolbox;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http.Json;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Diagnostics.Metrics;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.IdentityModel.Tokens;

global using OpenTelemetry;
global using OpenTelemetry.Metrics;
global using OpenTelemetry.Trace;

global using WebApi.Builders;
global using WebApi.Options;
global using WebApi.Services;
global using WebApi.Tokens;
global using WebApi.Utilities;
