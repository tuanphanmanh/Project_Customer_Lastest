using Abp.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Auditing
{
    public interface IExpiredAndDeletedAuditLogBackupService
    {
        bool CanBackup();

        void Backup(List<AuditLog> auditLogs);
    }
}
