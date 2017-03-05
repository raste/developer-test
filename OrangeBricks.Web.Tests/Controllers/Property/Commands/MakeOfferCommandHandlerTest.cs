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
    public class MakeOfferCommandHandlerTest
    {
        private MakeOfferCommandHandler _handler;
        private IOrangeBricksContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _handler = new MakeOfferCommandHandler(_context);

            var properties = new List<Models.Property>{
                new Models.Property{ Id = 1, StreetName = "Smith Street", Description = "", IsListedForSale = true },
                new Models.Property{ Id = 2, StreetName = "Jones Street", Description = "", IsListedForSale = true}
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>, IQueryable<Models.Property>>()
                                    .Initialize(properties.AsQueryable(), p => p.Id);

            _context.Properties.Returns(mockSet);
            _context.Offers.Returns(Substitute.For<IDbSet<Offer>>());
        }

        [Test]
        public void OfferIsCreatedAndItsDataIsCorrect()
        {
            // Arrange
            var command = new MakeOfferCommand
            {
                BuyerUserId = "1",
                Offer = 1,
                PropertyId = 1
            };

            // Act
            _handler.Handle(command);

            // Assert
            _context.Received(1).SaveChanges();

            var property = _context.Properties.FirstOrDefault(p => p.Id == 1);
            Assert.True(property.Offers != null);
            Assert.True(property.Offers.Count > 0);
            Assert.True(property.Offers.ElementAt(0).Amount == 1);
            Assert.True(property.Offers.ElementAt(0).BuyerUserId == "1");
        }
    }
}
