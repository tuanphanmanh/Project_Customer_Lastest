﻿using Abp.Application.Services;
using esign.Dto;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstEsignSystems.Exporting.Ver1
{
    public interface IMstEsignSystemsExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstEsignSystemsOutputDto> color);
    }
}