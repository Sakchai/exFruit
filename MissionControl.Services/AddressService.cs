using Microsoft.EntityFrameworkCore;
using MissionControl.Shared;
using MissionControl.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControl.Services
{
    public class AddressService :IAddressService
    {
        private readonly IRepository<Address> _addressRepository;
        public AddressService(IRepository<Address> addressRepository)
        {
            _addressRepository = addressRepository;
        }




        public Address GetAddressById(int id)
        {
            if (id == 0)
                return null;

            return _addressRepository.GetById(id);
        }

        public void InsertAddress(Address address)
        {
            _addressRepository.Insert(address);
        }

        public void UpdateAddress(Address address)
        {
            _addressRepository.Update(address);
        }
    }
}
