using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioBalta.Models
{
    [Table("IBGE")]
    public class IBGE
    {
        public string Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}