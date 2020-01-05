using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace MissionControl.Shared.Models.Purchase
{
  
    public class PurchaseListModel
    {
        public PurchaseModel[] PurchaseModel;

       // public IEnumerable<PurchaseModel> Query => _data.ToArray();


        //public PurchaseListModel(IList<PurchaseModel> data)
        //{
        //    _data = data;
        //}

        public int RecordsTotal { get; set; }
        public bool IsSuccessed { get; set; }
        public string ErrorMessage { get; set; }
    }
}
