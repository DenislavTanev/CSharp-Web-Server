namespace SharedTrip.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class User
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MinLength(UserMinUsername)]
        [MaxLength(UserMaxUsername)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(UserMinPassword)]
        public string Password { get; set; }

        public ICollection<UserTrip> UserTrips { get; set; }
    }
}
