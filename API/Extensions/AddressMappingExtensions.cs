using API.Dtos;
using Core.Enitities.Identity;

namespace API.Extensions
{
    public static class AddressMappingExtensions
    {
        public static void UpdateFromDto(this Address address, AddressDto addressDto)
        {
            if (addressDto == null) throw new ArgumentNullException(nameof(addressDto));
            if (address == null) throw new ArgumentNullException(nameof(address));

            address.Line1 = addressDto.Line1;
            address.Line2 = addressDto.Line2;
            address.City = addressDto.City;
            address.State = addressDto.State;
            address.Country = addressDto.Country;
            address.PostalCode = addressDto.PostalCode;
        }
    }
}
