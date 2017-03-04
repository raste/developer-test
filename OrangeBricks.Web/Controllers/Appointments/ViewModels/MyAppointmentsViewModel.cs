using System;
using System.Collections.Generic;

using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments.ViewModels
{
    public class MyAppointmentsViewModel
    {
        public bool HasRequestedAppointments { get { return Appointments != null
                                                            && Appointments.Count > 0; } }
        public List<PropertyAppointmentState> Appointments { get; set; }
    }

    public class PropertyAppointmentState
    {
        public string PropertyType { get; set; }
        public string PropertyStreetName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}