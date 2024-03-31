using NUnit.Framework;
using Moq;
using Application;
using Application.Models;
using System.Collections.Generic;
using Serilog;
using Application.Enums;

namespace ApplicationTests
{
    [TestFixture]
    public class ServerAcceptanceTests
    {
        private Mock<IDishManager> _dishManagerMock;
        private Server _sut;
        private Mock<ILogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger>();
            var dishManagerMock = new Mock<IDishManager>();
            _sut = new Server(_loggerMock.Object, new DishManager(_loggerMock.Object));
        }

        [TestCase("morning,1,2,3", "egg,toast,coffee")]
        [TestCase("morning,3,3,3", "coffee(x3)")]
        [TestCase("morning,1,3,2,3", "egg,toast,coffee(x2)")]
        [TestCase("morning,1,2,2", "error")]
        [TestCase("morning,1,2,4", "error")]
        [TestCase("evening,1,2,3,4", "steak,potato,wine,cake")]
        [TestCase("evening,1,2,2,4", "steak,potato(x2),cake")]
        [TestCase("evening,1,2,3,5", "error")]
        [TestCase("evening,1,1,2,3", "error")]
        public void TakeOrder_Returns_Correct_Output(string order, string expected)
        {
            // Arrange
            var parts = order.Split(',');
            var timeOfDay = parts[0].Trim().ToLower() == "morning" ? TimeOfDay.Morning : TimeOfDay.Evening;
            var dishes = string.Join(",", parts[1..]);

            // Act
            var actual = _sut.TakeOrder(dishes, timeOfDay);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private List<Dish> GetMockDishes(string order)
        {
            switch (order)
            {
                case "morning,1,2,3":
                    return new List<Dish>
                    {
                        new Dish { DishName = "egg", Count = 1 },
                        new Dish { DishName = "toast", Count = 1 },
                        new Dish { DishName = "coffee", Count = 1 }
                    };
                case "morning,3,3,3":
                    return new List<Dish>
                    {
                        new Dish { DishName = "coffee", Count = 3 }
                    };
                case "morning,1,3,2,3":
                    return new List<Dish>
                    {
                        new Dish { DishName = "egg", Count = 1 },
                        new Dish { DishName = "coffee", Count = 2 },
                        new Dish { DishName = "toast", Count = 1 }
                    };
                // Add cases for other orders
                default:
                    return new List<Dish>();
            }
        }
    }
}
