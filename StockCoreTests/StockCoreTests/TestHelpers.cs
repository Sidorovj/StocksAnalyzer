using StocksAnalyzer;
using Xunit;

namespace StockCoreTests
{
	public class TestHelpers
	{
		[Theory]
		[InlineData("a", "b")]
		public void SerializerTest(string s1, string s2)
		{
			string tmpFileName = "tmp3242343242.dat";
			Serializer ser = new Serializer(tmpFileName);
			ser.Serialize(new[] { "a", "b" });
			var des = ser.Deserialize();

			Assert.True(des is string[]);
			var strs = des as string[];
			Assert.Equal("a", strs[0]);
			Assert.Equal("b", strs[1]);
		}

		[Fact]
		public void StringParserTest()
		{

		}
	}
}
