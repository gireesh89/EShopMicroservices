using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            var scope= app.ApplicationServices.CreateScope();
            var dbcontext= scope.ServiceProvider.GetRequiredService<DiscountContext>();
            dbcontext.Database.MigrateAsync();
            return app;
        }
    }
}
