using System.Collections.Generic;
using System.Linq;
using Application;
using Application.Enums;
using Application.Models;
using Moq;
using NUnit.Framework;
using Serilog;

namespace ApplicationTests
{
    [TestFixture]
    public class DishManagerTests
    {
        private DishManager _sut;
        private Mock<ILogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger>();
            _sut = new DishManager(_loggerMock.Object);
        }

        [Test]
        public void EmptyListReturnsEmptyList()
        {
            var order = new Order();
            var actual = _sut.GetDishes(order, TimeOfDay.Morning);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void ListWith1ReturnsOneSteak()
        {
            var order = new Order
            {
                Dishes = new List<int>
                {
                    1
                }
            };

            var actual = _sut.GetDishes(order, TimeOfDay.Evening);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("steak", actual.First().DishName);
            Assert.AreEqual(1, actual.First().Count);
        }
    }
}
