using System.Collections.Generic;
using Abp.Runtime.Validation;
using esign.Dto;

namespace esign.Authorization.Users.Dto.Ver1
{
    public class GetUsersInput : PagedAndSortedInputDto, IShouldNormalize, IGetUsersInput
    {
        public string Filter { get; set; }

        public List<string> Permissions { get; set; }

        public int? Role { get; set; }

        public bool OnlyLockedUsers { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name,Surname";
            }

            Filter = Filter?.Trim();
        }
    }
}
