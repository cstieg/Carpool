using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpool.Domain.Models
{
    public class Vehicle : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [ForeignKey("ModelYear")]
        public int ModelYearId { get; set; }
        public virtual VehicleModelYear ModelYear { get; set; }

        public string Condition { get; set; }

        public string Description { get; set; }
    }
}
