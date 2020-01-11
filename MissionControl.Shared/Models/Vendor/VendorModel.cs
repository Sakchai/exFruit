using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MissionControl.Shared.Models
{
    /// <summary>
    /// Represents an purchase model
    /// </summary>

    public partial class VendorModel : BaseEntityModel
    {
        #region Ctor


        #endregion

        #region Properties

        //identifiers

        public string Name { get; set; } = "";

        #endregion


    }


}