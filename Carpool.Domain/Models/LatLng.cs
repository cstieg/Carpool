using System.ComponentModel.DataAnnotations;

namespace Carpool.Domain.Models
{
    public class LatLng : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public float Lat { get; set; }

        [Required]
        public float Lng { get; set; }
    }
}
