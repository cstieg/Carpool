using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpool.Domain.Models
{
    public class VehicleModel : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Make")]
        public int MakeId { get; set; }
        public virtual VehicleMake Make { get; set; }

        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return Make.ToString() + " " + Name;
        }
    }
}
