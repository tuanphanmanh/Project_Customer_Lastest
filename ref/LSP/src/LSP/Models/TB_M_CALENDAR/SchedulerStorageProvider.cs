using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace LSP.Models.TB_M_CALENDAR
{
    #region #SchedulerStorageProvider
    public class SchedulerStorageProvider
    {
        static MVCxAppointmentStorage defaultAppointmentStorage;
        public static MVCxAppointmentStorage DefaultAppointmentStorage
        {
            get
            {
                if (defaultAppointmentStorage == null)
                    defaultAppointmentStorage = CreateDefaultAppointmentStorage();
                return defaultAppointmentStorage;
            }
        }

        static MVCxAppointmentStorage CreateDefaultAppointmentStorage()
        {     
            MVCxAppointmentStorage appointmentStorage = new MVCxAppointmentStorage();
            appointmentStorage.Mappings.AppointmentId = "ID";
            appointmentStorage.Mappings.Start = "STARTDATE";
            appointmentStorage.Mappings.End = "ENDDATE";
            appointmentStorage.Mappings.Subject = "SUBJECT";
            appointmentStorage.Mappings.Description = "DESCRIPTION";
            appointmentStorage.Mappings.Location = "LOCATION";
            appointmentStorage.Mappings.AllDay = "AllDay";
            appointmentStorage.Mappings.Type = "TYPE";
            appointmentStorage.Mappings.RecurrenceInfo = "RECURRENCEINFO";
            appointmentStorage.Mappings.ReminderInfo = "REMINDERINFO";
            appointmentStorage.Mappings.Label = "LABEL";
            appointmentStorage.Mappings.Status = "STATUS";
            appointmentStorage.Mappings.ResourceId = "RESOURCEID";
            return appointmentStorage;
        }

        static MVCxResourceStorage defaultResourceStorage;
        public static MVCxResourceStorage DefaultResourceStorage
        {
            get
            {
                if (defaultResourceStorage == null)
                    defaultResourceStorage = CreateDefaultResourceStorage();
                return defaultResourceStorage;
            }
        }
        static MVCxResourceStorage CreateDefaultResourceStorage()
        {
            MVCxResourceStorage resourceStorage = new MVCxResourceStorage();
            resourceStorage.Mappings.ResourceId = "RESOURCEID";
            resourceStorage.Mappings.Caption = "RESOURCENAME";
            return resourceStorage;
        }
    }
    #endregion #SchedulerStorageProvider
}