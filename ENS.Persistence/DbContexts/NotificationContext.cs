using ENS.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace ENS.Persistence.DbContexts;
internal class NotificationContext : DbContext
{
    public NotificationContext(DbContextOptions<NotificationContext> options):base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserChannel> UserChannels { get; set; }
    public DbSet<TemplateChannel> TemplateChannels { get; set; }
    public DbSet<Template> Templates { get; set; }
}
