namespace SuperSimpleArchitecture.Fitnet.Passes.MarkPassAsExpired;

using Data.Database;
using Shared.SystemClock;

internal static class MarkPassAsExpiredEndpoint
{
    internal static void MapMarkPassAsExpired(this IEndpointRouteBuilder app)
    {
        app.MapPatch(PassesApiPaths.MarkPassAsExpired, async (Guid id, PassesPersistence persistence, ISystemClock systemClock,  CancellationToken cancellationToken) =>
        {
            var pass = await persistence.Passes.FindAsync(id, cancellationToken);
            if (pass is null)
                return Results.NotFound();

            pass.MarkAsExpired(systemClock.Now);
            await persistence.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        });
    }
}