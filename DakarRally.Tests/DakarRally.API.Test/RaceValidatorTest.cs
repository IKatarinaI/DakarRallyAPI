using DakarRally.API.Validators;
using NUnit.Framework;
using System.Linq;
using static DakarRally.Models.Enums;

namespace DakarRally.Tests.DakarRally.API.Test
{
    [TestFixture]
    public class RaceValidatorTest
    {
        [Test]
        public void ValidateForRaceValidationHelper_ReturnsNoValidationResultForValidInput()
        {
            var classUnderTest = new RaceValidator() { OldStatus = RaceStatus.Pending, Status = RaceStatus.Running };
            var result = classUnderTest.Validate(null).ToList();

            Assert.IsTrue(result.Count == 0);
        }

        [TestCase(RaceStatus.Running, RaceStatus.Finished)]
        [TestCase(RaceStatus.Finished, RaceStatus.Pending)]
        public void ValidateForRaceValidationHelper_ReturnsValidationResultForInvalidInput(RaceStatus oldStatus, RaceStatus newStatus)
        {
            var classUnderTest = new RaceValidator() { OldStatus = oldStatus, Status = newStatus };
            var result = classUnderTest.Validate(null).ToList();

            Assert.IsTrue(result.Count == 1);
        }

    }
}
