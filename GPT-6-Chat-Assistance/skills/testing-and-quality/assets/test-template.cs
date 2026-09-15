using NUnit.Framework;
using Moq;
using System.Threading.Tasks;

namespace <ProjectName>Core.Tests
{
    [TestFixture]
    public class ExampleServiceTests
    {
        private Mock<IExampleRepository> _mockRepository;
        private ExampleService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IExampleRepository>();
            _service = new ExampleService(_mockRepository.Object);
        }

        [Test]
        public async Task MethodName_Scenario_ExpectedResult()
        {
            // Arrange
            var expectedValue = true;
            _mockRepository.Setup(x => x.DoWorkAsync()).ReturnsAsync(expectedValue);

            // Act
            var result = await _service.ExecuteAsync();

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(x => x.DoWorkAsync(), Times.Once);
        }
    }
}
