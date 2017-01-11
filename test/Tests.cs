using System;
using Xunit;
using Discord.Addons.EmojiTools;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Parse_Unicode()
        {
            Assert.Equal("??", UnicodeEmoji.FromText(":racing_car:"));
        }
        [Fact]
        public void Parse_Unicode_Alias()
        {
            Assert.Equal("??", UnicodeEmoji.FromText(":camel:"));
        }
        [Fact]
        public void Parse_Invalid_Key()
        {
            Assert.Throws<ArgumentException>(() => UnicodeEmoji.FromText(":i really wish this was an emoji:"));
        }
    }
}