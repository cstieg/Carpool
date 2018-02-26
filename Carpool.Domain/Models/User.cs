using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpool.Domain.Models
{
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public Guid IdentityUserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsDriver { get; set; }

        public bool IsRider { get; set; }

        [ForeignKey("DefaultRideCost")]
        public int? DefaultRideCostId { get; set; }
        public RideCost DefaultRideCost { get; set; }

        [InverseProperty("Owner")]
        public ICollection<Location> Locations { get; set; }

        [InverseProperty("Owner")]
        public ICollection<Vehicle> Vehicles { get; set; }

        [InverseProperty("Creator")]
        public ICollection<Ride> Rides { get; set; }
    }
}
