using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace esign.EntityFrameworkCore
{
    public static class esignDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<esignDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<esignDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}