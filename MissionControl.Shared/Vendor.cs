using MissionControl.Shared.Enum;
using System.Collections.Generic;

namespace MissionControl.Shared
{
    /// <summary>
    /// Represents a vendor
    /// </summary>
    public partial class Vendor : BaseEntity
    {

        public string VendorCode { get; set; }

        public string TaxID { get; set; }
        public int CountryTypeId { get; set; }
        public CountryType CountryType
        {
            get => (CountryType)CountryTypeId;
            set => CountryTypeId = (int)value;
        }
        public int TaxTypeId { get; set; }

        public TaxType TaxType { 
            get => (TaxType)TaxTypeId; 
            set => TaxTypeId = (int)value; 
        }

        public int CompanyTypeId { get; set; }

        public CompanyType CompanyType
        {
            get => (CompanyType)CompanyTypeId;
            set => CompanyTypeId = (int)value;
        }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address identifier
        /// </summary>
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        public string Telephone { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

    

    }
}
