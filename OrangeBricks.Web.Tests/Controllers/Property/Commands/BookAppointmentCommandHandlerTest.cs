using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using NSubstitute;
using NUnit.Framework;

using OrangeBricks.Web.Controllers.Property.Commands;
using OrangeBricks.Web.Models;
using OrangeBricks.Web.Tests.Controllers.Property.Builders;

namespace OrangeBricks.Web.Tests.Controllers.Property.Commands
{
    [TestFixture]
    public class BookAppointmentCommandHandlerTest
    {
        private BookAppointmentCommandHandler _handler;
        private IOrangeBricksContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _handler = new BookAppointmentCommandHandler(_context);

            var properties = new List<Models.Property>{
                new Models.Property{ Id = 1, StreetName = "Smith Street", Description = "", IsListedForSale = true },
                new Models.Property{ Id = 2, StreetName = "Jones Street", Description = "", IsListedForSale = true}
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>, IQueryable<Models.Property>>()
                                    .Initialize(properties.AsQueryable());

            _context.Properties.Returns(mockSet);
            _context.Appointments.Returns(Substitute.For<IDbSet<Appointment>>());
        }

        [Test]
        public void AppointmentIsCreatedAndItsDataIsCorrect()
        {
            var date = DateTime.Now;

            // Arrange
            var command = new BookAppointmentCommand
            {
                BuyerUserId = "1",
                Date = date,
                PropertyId = 1
            };

            // Act
            _handler.Handle(command);

            // Assert
            _context.Received(1).SaveChanges();

            var property = _context.Properties.FirstOrDefault(p => p.Id == 1);
            Assert.True(property.Appointments != null);
            Assert.True(property.Appointments.Count > 0);

            var notChangedProperty = _context.Properties.FirstOrDefault(p => p.Id != 1);
            Assert.True(notChangedProperty.Appointments == null 
                        || notChangedProperty.Appointments.Count == 0);

            var appointment = property.Appointments.ElementAt(0);
            Assert.True(appointment.Date == date);
            Assert.True(appointment.BuyerUserId == "1");
            Assert.True(appointment.Status == AppointmentStatus.Pending);
        }
    }
}
