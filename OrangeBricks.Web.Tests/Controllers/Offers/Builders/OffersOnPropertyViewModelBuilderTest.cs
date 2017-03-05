using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using NSubstitute;
using NUnit.Framework;

using OrangeBricks.Web.Controllers.Offers.Builders;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Tests.Controllers.Offers.Builders
{
    [TestFixture]
    public class OffersOnPropertyViewModelBuilderTest
    {
        private IOrangeBricksContext _context;
        private OffersOnPropertyViewModelBuilder _builder;
        private List<Models.Property> _properties;
        private List<Offer> _offers;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _builder = new OffersOnPropertyViewModelBuilder(_context);

            var offersFor1 = new List<Offer>()
            {
                new Offer() { Id = 1, Amount = 1, BuyerUserId = "1", CreatedAt = DateTime.Now.AddDays(-5), Status = OfferStatus.Accepted, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 2, Amount = 2, BuyerUserId = "2", CreatedAt = DateTime.Now.AddHours(3), Status = OfferStatus.Rejected, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 3, Amount = 3, BuyerUserId = "2", CreatedAt = DateTime.Now.AddHours(-28), Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 4, Amount = 4, BuyerUserId = "1", CreatedAt = DateTime.Now.AddDays(2), Status = OfferStatus.Rejected, UpdatedAt = DateTime.Now }
            };

            var offersFor2 = new List<Offer>()
            {
                new Offer() { Id = 5, Amount = 5, BuyerUserId = "2", CreatedAt = DateTime.Now, Status = OfferStatus.Accepted, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 6, Amount = 6, BuyerUserId = "1", CreatedAt = DateTime.Now.AddHours(-56), Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 7, Amount = 7, BuyerUserId = "1", CreatedAt = DateTime.Now.AddDays(-3), Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 8, Amount = 8, BuyerUserId = "2", CreatedAt = DateTime.Now.AddHours(-2), Status = OfferStatus.Rejected, UpdatedAt = DateTime.Now }
            };

            _offers = offersFor1.Union(offersFor2).ToList();

            _properties = new List<Models.Property>{
                new Models.Property { Id = 1, StreetName = "Smith Street", Description = "", IsListedForSale = true, Offers = offersFor1},
                new Models.Property { Id = 2, StreetName = "Jones Street", Description = "", IsListedForSale = true, Offers = offersFor2}
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>, IQueryable<Models.Property>>()
                                    .Initialize(_properties.AsQueryable(), p => p.Id);

            _context.Properties.Returns(mockSet);
        }

        [Test]
        public void OffersAreCorrectlyFetchedPerProperty()
        {
            // Arrange
            var expectedOffers = _properties.Where(p => p.Id == 1)
                                            .SelectMany(p => p.Offers)
                                            .ToList();

            // Act
            var model = _builder.Build(1);

            // Assert
            Assert.True(model.HasOffers);
            Assert.True(model.Offers.Count() == expectedOffers.Count);

            foreach(var offer in model.Offers)
            {
                var compareWith = expectedOffers.FirstOrDefault(o => o.Id == offer.Id);
                Assert.True(compareWith != null);
                Assert.True(offer.Amount == compareWith.Amount);
                Assert.True(offer.IsPending == (compareWith.Status == OfferStatus.Pending));
            }
        }
    }
}
