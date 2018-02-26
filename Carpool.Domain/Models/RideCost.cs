using System.ComponentModel.DataAnnotations;

namespace Carpool.Domain.Models
{
    public class RideCost : IEntity
    {
        [Key]
        public int Id { get; set; }

        public decimal BaseCost { get; set; }

        public decimal CostPerMile { get; set; }

        public decimal PickupCostPerMile { get; set; }

        public decimal PickupCostPerHour { get; set; }

        public int PickupDistanceMileLimit { get; set; }

        public int PickupTimeLimitMinutes { get; set; }
    }
}
