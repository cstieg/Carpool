using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpool.Domain.Models
{
    public class VehicleModelYear : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Model")]
        public int ModelId { get; set; }
        public VehicleModel Model { get; set; }

        [Required]
        public int Year { get; set; }

        public override string ToString()
        {
            return Year.ToString() + " " + Model.ToString();
        }
    }
}
