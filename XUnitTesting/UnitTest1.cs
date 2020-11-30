using FoundryCore;
using System;
using Xunit;

//https://www.meziantou.net/quick-introduction-to-xunitdotnet.htm#setting-up-a-test-pr

namespace testing
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var sum = 18 + 24;
            Assert.Equal(42, sum);
        }

        [Fact]
        public void Test2()
        {
            var prop2a = new FoProperty<double>("cost", 100);
            prop2a.toJson();
            Assert.Equal(100, prop2a.Value);
        }

        [Fact]
        public void Test3()
        {
            var prop = new FoProperty<double>("cost", 300);
            Assert.Equal(300, prop.Value);
            var xxx = prop.toJson();
            Assert.Contains("cost", xxx);
        }
    }
}
