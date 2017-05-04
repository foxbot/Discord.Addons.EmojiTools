using Discord.Addons.EmojiTools;
using System;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Parse_Unicode()
        {
            Assert.Equal("🏎", EmojiExtensions.FromText(":racing_car:").Name);
        }
        [Fact]
        public void Parse_Unicode_Alias()
        {
            Assert.Equal("🐫", EmojiExtensions.FromText(":camel:").Name);
        }
        [Fact]
        public void Parse_Invalid_Key()
        {
            Assert.Throws<ArgumentException>(() => EmojiExtensions.FromText(":i really wish this was an emoji:"));
        }
    }
}