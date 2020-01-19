using System.ComponentModel.DataAnnotations;

namespace MissionControl.Shared
{
    public class Address : BaseEntity
    {


        [Required, MaxLength(100)]
        public string Address1 { get; set; }

        [MaxLength(100)]
        public string District { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; }

        [Required, MaxLength(20)]
        public string Province { get; set; }

        [Required, MaxLength(20)]
        public string PostalCode { get; set; }
    }
}
