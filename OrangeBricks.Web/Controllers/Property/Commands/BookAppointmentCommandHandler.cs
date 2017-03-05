using System.Collections.Generic;
using System.Linq;

using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Property.Commands
{
    public class BookAppointmentCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        public BookAppointmentCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates appointment for the specified date and property.
        /// </summary>
        public void Handle(BookAppointmentCommand command)
        {
            var property = _context.Properties.SingleOrDefault(p => p.Id == command.PropertyId);

            var appointment = new Appointment
            {
                 Date = command.Date,
                 Status =  AppointmentStatus.Pending,
                 BuyerUserId = command.BuyerUserId
            };

            if (property.Appointments == null)
            {
                property.Appointments = new List<Appointment>();
            }

            property.Appointments.Add(appointment);

            _context.Appointments.Add(appointment);

            _context.SaveChanges();
        }
    }
}