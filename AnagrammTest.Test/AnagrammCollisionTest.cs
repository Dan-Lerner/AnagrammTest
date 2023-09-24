using Xunit.Abstractions;

namespace AnagrammTest.Test
{
    public class AnagrammCollisionTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public AnagrammCollisionTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ClassicTest()
        {
            // Arrange
            string strTest = "abcdefjhijklmnop";
            string strAnagramm = "bcdelfjhijkmnaop";
            string strNonAnagramm = "abcdevjhijklmnop";

            var anagramm = new Anagramm(strTest);

            // Act
            bool res1 = anagramm.IsAnagrammClassic(strAnagramm);
            bool res2 = anagramm.IsAnagrammClassic(strNonAnagramm);

            bool res3 = anagramm.IsAnagrammNonlinear(strAnagramm);
            bool res4 = anagramm.IsAnagrammNonlinear(strNonAnagramm);

            // Assert
            Assert.True(res1);
            Assert.False(res2);
            Assert.True(res3);
            Assert.False(res4);
        }

        [Fact]
        public void CollisionTest()
        {
            // !!! This test may take a lot of time
            // It depends on strTest.Length and max CodeValue

            // Arrange
            string strTest = "abcdefghijklmno";
            char maxCodeValue = 'w';
            char minCodeValue = 'a';

            var anagramm = new Anagramm(strTest);
            var testEqual = new FindEqualSumStrings(strTest, maxCodeValue, minCodeValue);

            int collisions = 0;
            string strEqual;

            // Act
            testEqual.Start(chars =>
            {
                strEqual = new string(chars);

                if (
                    anagramm.IsAnagrammNonlinear(strEqual) !=
                    //Anagramm.IsAnagrammLINQ(strTest, strEqual) !=
                    anagramm.IsAnagrammClassic(strEqual)
                    )
                {
                    collisions++;
                    testOutputHelper.WriteLine("Collision: \"{0}\" and \"{1}\"", strTest, strEqual);
                }

                return true;
            });

            // Assert
            Assert.True(collisions == 0);
        }
    }
}
