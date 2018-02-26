using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpool.Domain.Models
{
    public class Location : IEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public LatLng LatLng { get; set; }

        public Address Address { get; set; }
    }
}
