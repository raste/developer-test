using System.Linq;

using OrangeBricks.Web.Models;
using System.Data.Entity;

namespace OrangeBricks.Web.Controllers.Appointments.Commands
{
    public class UpdateAppointmentCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        public UpdateAppointmentCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Updates the status of appointment.
        /// </summary>
        public void Handle(UpdateAppointmentCommand command)
        {
            var property = _context.Properties.Where(p => p.Id == command.PropertyId
                                                                && p.SellerUserId == command.SellerUserId
                                                                && p.Appointments.Any(a => a.Id == command.AppointmentId))
                                              .Include(p => p.Appointments)
                                              .FirstOrDefault();


            var appointment = property.Appointments.First(a => a.Id == command.AppointmentId);

            appointment.Status = command.NewStatus;

            _context.SaveChanges();
        }
    }
}