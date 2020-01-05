using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.Shared.Enum
{
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
