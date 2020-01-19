using MissionControl.Shared;
using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MissionControl.Services
{
    public interface IAddressService
    {

        Address GetAddressById(int id);
        void UpdateAddress(Address address);
        void InsertAddress(Address address);
    }
}
