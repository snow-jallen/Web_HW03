using NUnit.Framework;
using Web_HW03.Controllers;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(2,3,5)]
        [TestCase(5,10,15)]
        [TestCase(1,2,5)]
        public void Test1(int num1, int num2, int expectedAnswer)
        {
            var actualAnswer = num1 + num2;

            Assert.AreEqual(expectedAnswer, actualAnswer);
        }
    }
}