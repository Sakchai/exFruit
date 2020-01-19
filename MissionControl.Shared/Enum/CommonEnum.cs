using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.Shared.Enum
{
    public enum CountryType
    {
        Thailand = 10,
        OtherCountry = 20
    }

    public enum TaxType
    {
        none = 10,
        HeadOffice = 20,
        Branch = 30
    }

    public enum CompanyType
    {
        Individual = 10,
        CompanyLimited = 20,
        PublicCompanyLimited = 30,
    }
    public enum PurchaseStatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 10,

        /// <summary>
        /// Complete
        /// </summary>
        Paid = 20,

        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 30
    }

    public enum PurchaseProcess
    {
        Purchase = 10,
        Receive = 20,
        Sort    = 30,
        Sorted =  40
    }
}
