using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.Shared.Models.Common
{
    public class CommonListModel
    {
        /// <summary>
        /// Page index
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Has previous page
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Has next age
        /// </summary>
        public bool HasNextPage { get; set; }

        public bool IsSuccessed { get; set; }
        public string ErrorMessage { get; set; } = "";
    }
}
