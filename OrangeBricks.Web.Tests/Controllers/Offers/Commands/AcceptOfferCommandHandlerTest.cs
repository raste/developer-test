using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using NSubstitute;
using NUnit.Framework;

using OrangeBricks.Web.Models;
using OrangeBricks.Web.Controllers.Offers.Commands;

namespace OrangeBricks.Web.Tests.Controllers.Offers.Commands
{
    [TestFixture]
    public class AcceptOfferCommandHandlerTest
    {
        private IOrangeBricksContext _context;
        private AcceptOfferCommandHandler _handler;
        private List<Offer> _offers;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _handler = new AcceptOfferCommandHandler(_context);

            var offersFor1 = new List<Offer>()
            {
                new Offer() { Id = 1, Amount = 1, BuyerUserId = "1", CreatedAt = DateTime.Now, Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 2, Amount = 2, BuyerUserId = "2", CreatedAt = DateTime.Now, Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
            };

            var offersFor2 = new List<Offer>()
            {
                new Offer() { Id = 5, Amount = 5, BuyerUserId = "2", CreatedAt = DateTime.Now, Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 6, Amount = 6, BuyerUserId = "1", CreatedAt = DateTime.Now, Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
            };

            _offers = offersFor1.Union(offersFor2).ToList();

            var properties = new List<Models.Property>{
                new Models.Property { Id = 1, StreetName = "Smith Street", Description = "", IsListedForSale = true, Offers = offersFor1},
                new Models.Property { Id = 2, StreetName = "Jones Street", Description = "", IsListedForSale = true, Offers = offersFor2}
            };

            var propertiesMockSet = Substitute.For<IDbSet<Models.Property>, IQueryable<Models.Property>>()
                                              .Initialize(properties.AsQueryable(), p => p.Id);

            _context.Properties.Returns(propertiesMockSet);

            var offersMockSet = Substitute.For<IDbSet<Offer>, IQueryable<Offer>>()
                                          .Initialize(_offers.AsQueryable(), o => o.Id);

            _context.Offers.Returns(offersMockSet);
        }

        [Test]
        public void OfferIsAccepted()
        {
            // Arrange
            var command = new AcceptOfferCommand
            {
                PropertyId = 2,
                OfferId = 6
            };

            // Act
            _handler.Handle(command);

            // Assert
            _context.Received(1).SaveChanges();

            Assert.True(_context.Offers.Count(o => o.Status == OfferStatus.Accepted) == 1);

            var offer = _context.Offers.FirstOrDefault(o => o.Id == 6);
            Assert.True(offer.Status == OfferStatus.Accepted);
        }
    }
}
