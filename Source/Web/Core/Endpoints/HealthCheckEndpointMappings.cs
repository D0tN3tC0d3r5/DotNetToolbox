using static WebApi.Endpoints.HealthCheckApiPaths;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

public static class HealthCheckEndpointMappings {
    public static IEndpointRouteBuilder MapHealthCheckEndpoints(this IEndpointRouteBuilder app) {
        app.MapHealthChecks(IsHealthy)
           .WithName("IsHealthy");
        app.MapHealthChecks(IsAlive, new() { Predicate = r => r.Tags.Contains("live") })
           .WithName("IsAlive");
        return app;
    }
}
