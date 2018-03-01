using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Carpool.Domain.Models
{
    public class Ride : IEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Creator")]
        public int CreatorUserId { get; set; }
        [Required]
        public virtual User Creator { get; set; }

        [ForeignKey("Driver")]
        public int? DriverUserId { get; set; }
        public virtual User Driver { get; set; }

        [InverseProperty("Ride")]
        public ICollection<Rider> Riders { get; set; }

        [ForeignKey("Vehicle")]
        public int? VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        [InverseProperty("Ride")]
        public ICollection<RideLeg> RideLegs { get; set; }

        [ForeignKey("RideCost")]
        public int? RideCostId { get; set; }
        public virtual RideCost RideCost { get; set; }

        public float Distance {
            get
            {
                return RideLegs.Sum(r => r.Distance);
            }
        }

        public TimeSpan TravelTime
        {
            get
            {
                return new TimeSpan(RideLegs.Sum(r => r.TravelTime.Ticks));
            }
        }

        /// <summary>
        /// Bitwise representation of days of the week that are repeated. Values enumerated in WeekDay enum
        /// </summary>
        public byte Repeating { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? AvailableSeats { get; set; }

        public int? RemainingSeats
        {
            get
            {
                return AvailableSeats - Riders.Count;
            }
        }

        public bool IsSeekingRiders { get; set; }

        public bool IsSeekingDriver { get; set; }
    }
}
