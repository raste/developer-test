using System;

namespace OrangeBricks.Web.Controllers.Property.Commands
{
    public class BookAppointmentCommand
    {
        public int PropertyId { get; set; }

        public DateTime Date { get; set; }

        public string BuyerUserId { get; set; }
    }
}