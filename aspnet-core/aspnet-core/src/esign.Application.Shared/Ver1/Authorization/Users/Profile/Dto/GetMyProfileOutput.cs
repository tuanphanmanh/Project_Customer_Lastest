using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class GetMyProfileOutput
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
    }
}
