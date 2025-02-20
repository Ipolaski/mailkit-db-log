using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Infrastructure.Entities
{
    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Path { get; set; }
    }
}