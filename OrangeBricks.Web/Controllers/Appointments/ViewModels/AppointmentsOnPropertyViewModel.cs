using System;
using System.Collections.Generic;

using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments.ViewModels
{
    public class AppointmentsOnPropertyViewModel
    {
        public int PropertyId { get; set; }
        public string PropertyType { get; set; }
        public string StreetName { get; set; }
        public int NumberOfBedrooms { get; set; }
        public bool HasAppointments { get { return Appointments != null
                                                   && Appointments.Count > 0; } }
        public List<PropertyAppointment> Appointments { get; set; }
    }

    public class PropertyAppointment
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}