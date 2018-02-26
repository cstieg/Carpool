using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpool.Domain.Models
{
    public class Rider : IEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Ride")]
        public int RideId { get; set; }
        public virtual Ride Ride { get; set; }
    }
}
