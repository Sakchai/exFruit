using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>
    public partial class ReceptionModel : BaseEntityModel
    {
        public IList<ReceptionItemModel> ReceptionItems { get; set; }

        #region Ctor

        public ReceptionModel()
        {
            ReceptionItems = new List<ReceptionItemModel>();
        }

        #endregion

        #region Properties

        public string ReceivedBy { get; set; }
        #endregion


    }


}