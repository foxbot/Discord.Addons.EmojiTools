# script to generate :foo: -> '<unicode_foo>' dict in c#
# requires 'emoji' from pip (https://github.com/carpedm20/emoji)
#
# by foxbot

import emoji
import io

template = """
using System.Collections.Generic;

namespace Discord.Addons.EmojiTools
{{
    public static class EmojiMap
    {{
        public static IReadOnlyDictionary<string, string> Map = new Dictionary<string, string> 
        {{ 
            {0} 
        }};
    }}
}}
"""

item = "[\"{key}\"] = \"{value}\",\n"

items = ""

for k, v in emoji.EMOJI_UNICODE.items():
    items += item.format(key = k, value = v)
for k, v in emoji.EMOJI_ALIAS_UNICODE.items():
    items += item.format(key = k, value = v)

final = template.format(items)

file = io.open('src/Discord.Addons.EmojiTools/EmojiMap.cs', 'w', encoding = 'utf16')
file.writelines(final)
file.close()