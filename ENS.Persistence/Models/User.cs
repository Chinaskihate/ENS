using System.ComponentModel.DataAnnotations;

namespace ENS.Persistence.Models;
internal class User
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<UserChannel> Channels { get; set; }
    public List<Template> Templates { get; set; }
}
