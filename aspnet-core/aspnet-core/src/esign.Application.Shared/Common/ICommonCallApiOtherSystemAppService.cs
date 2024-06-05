using Abp.Application.Services;
using esign.Business.Ver1;
using esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Common
{
    public interface ICommonCallApiOtherSystemAppService : IApplicationService
    {
        void UpdateResultForOtherSystem(UpdateRequestStatusToOrtherSystemDto updateRequestStatusToOrtherSystemDto);
        ResultForwardForEsignDto UpdateReassignForOtherSystem(ReassignRequestInputOtherSystemDto reassignRequestInputOtherSystemDto);
    }
}
