using MissionControl.Shared.Enum;
using System;
using System.Collections.Generic;


namespace MissionControl.Shared
{
    /// <summary>
    /// Represents an order
    /// </summary>
    public partial class Purchase : BaseEntity
    {

        private ICollection<PurchaseItem> _purchaseItems;


        #region Properties
        //public int Id { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public Guid PurchaseGuid { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int? VendorId { get; set; }

        /// <summary>
        /// Gets or sets the billing address identifier
        /// </summary>
        public int? BillingAddressId { get; set; }

        /// <summary>
        /// Gets or sets the shipping address identifier
        /// </summary>
        public int? ReceivingAddressId { get; set; }

        /// <summary>
        /// Gets or sets the pickup address identifier
        /// </summary>
        public int? PickupAddressId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a customer chose "pick up in store" shipping option
        /// </summary>
        public bool PickupInStore { get; set; }

        /// <summary>
        /// Gets or sets an order status identifier
        /// </summary>
        public int? PurchaseStatusId { get; set; }


        public int? PurchaseProcessId { get; set; }

        public string PurchaseNo { get; set; }
        /// <summary>
        /// Gets or sets the tax rates
        /// </summary>
        public string TaxRates { get; set; }

        /// <summary>
        /// Gets or sets the order tax
        /// </summary>
        public decimal PurchaseTax { get; set; }

        /// <summary>
        /// Gets or sets the order discount (applied to order total)
        /// </summary>
        public decimal PurchaseDiscount { get; set; }

        /// <summary>
        /// Gets or sets the order total
        /// </summary>
        public decimal PurchaseTotal { get; set; }


        /// <summary>
        /// Gets or sets the shipping method
        /// </summary>
        public string ReceivingMethod { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time of order creation
        /// </summary>
        public DateTime? CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the custom order number without prefix
        /// </summary>
        public string CustomPurchaseNumber { get; set; }

        public int TotalCrates { get; set; }

        public decimal? ActualCost { get; set; }

        #endregion

        #region Navigation properties

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Vendor Vendor { get; set; }

        /// <summary>
        /// Gets or sets the billing address
        /// </summary>
        public virtual Address BillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the shipping address
        /// </summary>
        public virtual Address ReceivingAddress { get; set; }

        /// <summary>
        /// Gets or sets the pickup address
        /// </summary>
        public virtual Address PickupAddress { get; set; }



        /// <summary>
        /// Gets or sets order items
        /// </summary>
        public virtual ICollection<PurchaseItem> PurchaseItems
        {
            get => _purchaseItems ?? (_purchaseItems = new List<PurchaseItem>());
            protected set => _purchaseItems = value;
        }

        /// <summary>
        /// Gets or sets the order status
        /// </summary>
        public PurchaseStatus PurchaseStatus
        {
            get => (PurchaseStatus)PurchaseStatusId;
            set => PurchaseStatusId = (int)value;
        }

        public PurchaseProcess PurchaseProcess
        {
            get => (PurchaseProcess)PurchaseProcessId;
            set => PurchaseProcessId = (int)value;
        }
        #endregion

    }
}