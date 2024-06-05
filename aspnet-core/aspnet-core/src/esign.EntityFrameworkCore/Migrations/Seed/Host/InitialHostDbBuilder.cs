using esign.EntityFrameworkCore;

namespace esign.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly esignDbContext _context;

        public InitialHostDbBuilder(esignDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
