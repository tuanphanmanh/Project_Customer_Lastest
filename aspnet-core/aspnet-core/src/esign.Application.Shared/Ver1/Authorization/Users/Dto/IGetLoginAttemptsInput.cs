using Abp.Application.Services.Dto;

namespace esign.Authorization.Users.Dto.Ver1
{
    public interface IGetLoginAttemptsInput: ISortedResultRequest
    {
        string Filter { get; set; }
    }
}