using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpool.Domain.Models
{
    public class RideLeg : IEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ride")]
        public int RideId { get; set; }
        public Ride Ride { get; set; }

        [Required]
        [ForeignKey("Origin")]
        public int OriginId { get; set; }
        public virtual Location Origin { get; set; }

        [Required]
        [ForeignKey("Destination")]
        public int DestinationId { get; set; }
        public virtual Location Destination { get; set; }

        // distance in miles
        public float Distance { get; set; }

        public TimeSpan TravelTime { get; set; }

        // have to use TimeSpan instead of DateTime without date to avoid errors in SQL server
        public TimeSpan StartTime { get; set; }

        public TimeSpan ArrivalTime {
            get
            {
                return StartTime + TravelTime;
            }
        }
    }
}
