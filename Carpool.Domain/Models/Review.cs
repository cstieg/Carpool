using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpool.Domain.Models
{
    public class Review : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [ForeignKey("Reviewee")]
        public int RevieweeId { get; set; }
        public virtual User Reviewee { get; set; }

        public bool RevieweeWasDriver { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int Stars { get; set; }

        public string Comments { get; set; }
    }
}
