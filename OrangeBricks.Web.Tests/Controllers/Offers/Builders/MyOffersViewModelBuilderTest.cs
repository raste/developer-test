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
    public class MyOffersViewModelBuilderTest
    {
        private IOrangeBricksContext _context;
        private MyOffersViewModelBuilder _builder;
        private List<Offer> _offers;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _builder = new MyOffersViewModelBuilder(_context);

            var offersFor1 = new List<Offer>()
            {
                new Offer() { Id = 1, Amount = 1, BuyerUserId = "1", CreatedAt = DateTime.Now.AddDays(-5), Status = OfferStatus.Accepted, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 2, Amount = 2, BuyerUserId = "2", CreatedAt = DateTime.Now, Status = OfferStatus.Rejected, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 3, Amount = 3, BuyerUserId = "2", CreatedAt = DateTime.Now, Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 4, Amount = 4, BuyerUserId = "1", CreatedAt = DateTime.Now.AddDays(2), Status = OfferStatus.Rejected, UpdatedAt = DateTime.Now }
            };

            var offersFor2 = new List<Offer>()
            {
                new Offer() { Id = 5, Amount = 5, BuyerUserId = "2", CreatedAt = DateTime.Now, Status = OfferStatus.Accepted, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 6, Amount = 6, BuyerUserId = "1", CreatedAt = DateTime.Now, Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 7, Amount = 7, BuyerUserId = "1", CreatedAt = DateTime.Now.AddDays(-3), Status = OfferStatus.Pending, UpdatedAt = DateTime.Now }
                , new Offer() { Id = 8, Amount = 8, BuyerUserId = "2", CreatedAt = DateTime.Now, Status = OfferStatus.Rejected, UpdatedAt = DateTime.Now }
            };

            _offers = offersFor1.Union(offersFor2).ToList();

            var properties = new List<Models.Property>{
                new Models.Property { Id = 1, StreetName = "Smith Street", Description = "", IsListedForSale = true, Offers = offersFor1},
                new Models.Property { Id = 2, StreetName = "Jones Street", Description = "", IsListedForSale = true, Offers = offersFor2}
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>, IQueryable<Models.Property>>()
                                    .Initialize(properties.AsQueryable(), p => p.Id);

            _context.Properties.Returns(mockSet);
        }

        [Test]
        public void OffersAreCorrectlyFetchedPerUser()
        {
            // Arrange
            var expectedOffers = _offers.Where(o => o.BuyerUserId == "1")
                                        .OrderByDescending(o => o.CreatedAt)
                                        .ToList();

            // Act
            var model = _builder.Build("1");

            // Assert
            Assert.True(model.HasMadeOffers);
            Assert.True(model.OffersStates.Count == expectedOffers.Count);

            for(int i = 0; i < expectedOffers.Count; i++)
            {
                Assert.True(expectedOffers[i].Status == model.OffersStates[i].OfferStatus);
                Assert.True(expectedOffers[i].Amount == model.OffersStates[i].OfferAmount);
            }
        }
    }
}
