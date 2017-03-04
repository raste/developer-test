using OrangeBricks.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrangeBricks.Web.Controllers.Appointments.Commands
{
    public class UpdateAppointmentCommand
    {
        public string SellerUserId { get; set; }

        public int AppointmentId { get; set; }

        public int PropertyId { get; set; }

        public AppointmentStatus NewStatus { get; set; }
    }
}