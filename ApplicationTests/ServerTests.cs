using Application;
using Application.Enums;
using Moq;
using NUnit.Framework;
using Serilog;

namespace ApplicationTests
{
    [TestFixture]
    public class ServerTests
    {
        private Server _sut;
        private Mock<ILogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger>();
            var dishManagerMock = new Mock<IDishManager>();
            _sut = new Server(_loggerMock.Object, new DishManager(_loggerMock.Object));
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void ErrorGetsReturnedWithBadInput()
        {
            var order = "one";
            string expected = "error";
            var actual = _sut.TakeOrder(order, TimeOfDay.Morning);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServeSteak()
        {
            var order = "1";
            string expected = "steak";
            var actual = _sut.TakeOrder(order, TimeOfDay.Evening);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServe2Potatoes()
        {
            var order = "2,2";
            string expected = "potato(x2)";
            var actual = _sut.TakeOrder(order, TimeOfDay.Evening);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServeSteakPotatoWineCake()
        {
            var order = "1,2,3,4";
            string expected = "steak,potato,wine,cake";
            var actual = _sut.TakeOrder(order, TimeOfDay.Evening);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServeSteakPotatox2Cake()
        {
            var order = "1,2,2,4";
            string expected = "steak,potato(x2),cake";
            var actual = _sut.TakeOrder(order, TimeOfDay.Evening);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanGenerateErrorWithWrongDish()
        {
            var order = "1,2,3,5";
            string expected = "error";
            var actual = _sut.TakeOrder(order, TimeOfDay.Evening);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanGenerateErrorWhenTryingToServerMoreThanOneSteak()
        {
            var order = "1,1,2,3";
            string expected = "error";
            var actual = _sut.TakeOrder(order, TimeOfDay.Evening);
            Assert.AreEqual(expected, actual);
        }
    }
}