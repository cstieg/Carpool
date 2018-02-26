using System.ComponentModel.DataAnnotations;

namespace Carpool.Domain.Models
{
    public class VehicleMake : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
