using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var serviceprovider = scope.ServiceProvider;
                var db = serviceprovider.GetRequiredService<TournamentApiContext>();

                await db.Database.MigrateAsync();

                if (await db.TournamentDetails.AnyAsync()) return;

                try
                {
                    var tournamentDetails = SeedData.GenerateTournamentDetails();
                    db.AddRange(tournamentDetails);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) 
                {
                    throw;
                }
            }
        }
    }
}
