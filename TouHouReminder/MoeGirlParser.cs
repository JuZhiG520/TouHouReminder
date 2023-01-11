using System.Text.RegularExpressions;

namespace TouHouReminder
{
    internal static class MoeGirlParser
    {
        private static readonly Dictionary<(FestivalType, int), string> CHARACTER_DICTIONARY = new()
        {
            { (FestivalType.Days, 5), "TH1 (Highly Responsive to Prayers)" },
            { (FestivalType.Days, 8), "TH2 (The Story of Eastern Wonderland)" },
            { (FestivalType.Days, 10), "Ellen" },
            { (FestivalType.Days, 11), "TH3 (Phantasmagoria of Dim.Dream)" },
            { (FestivalType.Days, 20), "Sunny Milk" },
            { (FestivalType.Days, 23), "Miko" },
            { (FestivalType.Days, 26), "Myouren" },
            { (FestivalType.Days, 32), "ZUN" },
            { (FestivalType.Days, 40), "Hoan Meirin & Remilia Scarlet" },
            { (FestivalType.Days, 41), "TH5 (Mystic Square)" },
            { (FestivalType.Days, 49), "TH13.5 (Hopeless Masquerade)" },
            { (FestivalType.Days, 58), "Karakasa" },
            { (FestivalType.Days, 63), "Nameless Characters" },
            { (FestivalType.Days, 90), "TH10 (Mountain of Faith)" },
            { (FestivalType.Days, 96), "Sister Characters" },
            { (FestivalType.Days, 103), "Lizards" },
            { (FestivalType.Days, 104), "Aizome" },
            { (FestivalType.Days, 108), "Kurumi" },
            { (FestivalType.Days, 109), "Curiosities of Lotus Asia" },
            { (FestivalType.Days, 118), "Tengu" },
            { (FestivalType.Days, 122), "TH13 (Ten Desires)" },
            { (FestivalType.Days, 131), "Reiuji Utsuho & Kaenbyou Rin & Reisen Udongein Inaba & Tewi Inaba" },
            { (FestivalType.Days, 132), "Pocky" },
            { (FestivalType.Days, 135), "Ghostly Field Club" },
            { (FestivalType.DaysEveryMonth, 23), "Sunny Milk" },
            { (FestivalType.DaysEveryMonth, 26), "Houraijin" },
            { (FestivalType.DaysEveryMonth, 28), "Ellen" },
            { (FestivalType.DaysEveryMonth, 29), "Mima" },
            { (FestivalType.DaysEveryWeek, 0), "Lunar Capital Characters" }
        };

        private static readonly Dictionary<(FestivalType, int), string> FESTIVAL_NAME_DICTIONARY = new()
        {
            { (FestivalType.Days, 0), "The Professor's Day" },
            { (FestivalType.Days, 4), "Iku San's Day" },
            { (FestivalType.Days, 7), "The Ordinary Magician's Day" },
            { (FestivalType.Days, 9), "Miko Sama's Day" },
            { (FestivalType.Days, 15), "Neet's Day" },
            { (FestivalType.Days, 18), "Carrion's Day" },
            { (FestivalType.Days, 21), "Hina's Festival" },
            { (FestivalType.Days, 22), "Myon's Day" },
            { (FestivalType.Days, 23), "Miko's Day" },
            { (FestivalType.Days, 25), "Sanae Chan's Day" },
            { (FestivalType.Days, 27), "Moriya Sama's Day" },
            { (FestivalType.Days, 32), "ZUN's Birthday" },
            { (FestivalType.Days, 38), "Shirasawa's Day" },
            { (FestivalType.Days, 39), "Conparo's Festival" },
            { (FestivalType.Days, 40), "Melee's Day" },
            { (FestivalType.Days, 47), "Master Spark's Day" },
            { (FestivalType.Days, 57), "The festival for the people who did not catch Patchouli's Day" },
            { (FestivalType.Days, 58), "Karakasa's Day" },
            { (FestivalType.Days, 59), "Lolita Koishi's Day" },
            { (FestivalType.Days, 63), "Nameless Characters' Day" },
            { (FestivalType.Days, 64), "Namusan's Day" },
            { (FestivalType.Days, 69), "Iku Sama's Day" },
            { (FestivalType.Days, 72), "The Seven-colored Puppeteer's Day" },
            { (FestivalType.Days, 85), "Super Yatsuhashi's Day" },
            { (FestivalType.Days, 86), "Yakujin's Day" },
            { (FestivalType.Days, 87), "Hakurei's Day" },
            { (FestivalType.Days, 92), "Perfect's Day" },
            { (FestivalType.Days, 96), "Sister Characters' Day" },
            { (FestivalType.Days, 98), "Tsukumo Sisters' Day" },
            { (FestivalType.Days, 101), "Cirno Time" },
            { (FestivalType.Days, 103), "Lizards' Festival" },
            { (FestivalType.Days, 104), "Aizome Hiyori" },
            { (FestivalType.Days, 106), "Tsukumo Sisters' Day" },
            { (FestivalType.Days, 109), "Kourindou Release Anniversary" },
            { (FestivalType.Days, 116), "The festival for the people who did not catch Tenshi's Day" },
            { (FestivalType.Days, 117), "Soudesuka's Day" },
            { (FestivalType.Days, 118), "Tengu's Day" },
            { (FestivalType.Days, 120), "Super Ichirin Time" },
            { (FestivalType.Days, 128), "Koishi Koishi's Day" },
            { (FestivalType.Days, 130), "Onbashira's Day" },
            { (FestivalType.Days, 131), "Four Spell Cards' Day" },
            { (FestivalType.Days, 132), "TouHou Pocky's Day" },
            { (FestivalType.Days, 137), "Expressionless Day" },
            { (FestivalType.Days, 138), "Yakujin's Christmastime" },
            { (FestivalType.DaysEveryMonth, 8), "The festival for the people who did not catch Rumia's Day" },
            { (FestivalType.DaysEveryMonth, 13), "Kyuubi's Day" },
            { (FestivalType.DaysEveryMonth, 16), "The Professor's Day" },
            { (FestivalType.DaysEveryMonth, 21), "Iku Sama's Day" },
            { (FestivalType.DaysEveryMonth, 23), "Sun Day" },
            { (FestivalType.DaysEveryMonth, 26), "Immortal Day" },
            { (FestivalType.DaysEveryMonth, 29), "Mima Sama's Day" },
            { (FestivalType.DaysEveryWeek, 0), "Lunar Capital Characters' Day" },
        };

        public static FestivalCollection ParseMoeGirlCode()
        {
            FestivalCollection collection = new();

            using HttpClient client = new();
            string code = client.GetStringAsync("https://zh.moegirl.org.cn/%E4%B8%9C%E6%96%B9%E8%A7%92%E8%89%B2%E6%97%A5").Result;

            MatchCollection[] matchCollections = new MatchCollection[Enum.GetNames(typeof(FestivalType)).Length];

            matchCollections[(int)FestivalType.Days] =
                Regex.Matches(code, "<tr>\\r?\\n?<td>\\d+月\\S+日\\S*</td>\\r?\\n?<td>(\\S| )+\\r?\\n?</td></tr>");

            matchCollections[(int)FestivalType.DaysEveryMonth] =
                Regex.Matches(code, "<tr>\\r?\\n?<td>每月\\S+日\\S*</td>\\r?\\n?<td>(\\S| )+\\r?\\n?</td></tr>");

            matchCollections[(int)FestivalType.DaysEveryWeek] =
                Regex.Matches(code, "<tr>\\r?\\n?<td>\\S+日（星期\\S）</td>\\r?\\n?<td>(\\S| )+\\r?\\n?</td></tr>");

            matchCollections[(int)FestivalType.Months] =
                Regex.Matches(code, "<tr>\\r?\\n?<td>\\d+月</td>\\r?\\n?<td>(\\S| )+\\r?\\n?</td></tr>");

            for (int i = 0; i < matchCollections.Length; i++)
            {
                for (int j = 0; j < matchCollections[i].Count; j++)
                {
                    string[] strArray = matchCollections[i][j].Value.Split('\n');
                    string time = string.Empty, character = string.Empty, name = string.Empty;

                    if (Regex.IsMatch(strArray[1], "</?td>"))
                    {
                        time = Regex.Replace(strArray[1], "</?td>", string.Empty);
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (!strArray[2].Contains("</span>"))
                    {
                        if (Regex.IsMatch(strArray[2], "title=\"\\S+\""))
                        {
                            MatchCollection matches = Regex.Matches(strArray[2], "title=\"\\S+\"");

                            for (int k = 0; k < matches.Count; k++)
                            {
                                character += matches[k].Value[(matches[k].Value.IndexOf('\"') + 1)..matches[k].Value.LastIndexOf('\"')];

                                if (k < matches.Count - 1)
                                {
                                    character += " & ";
                                }
                            }
                        }

                        if (Regex.IsMatch(strArray[2], "[^\\s<>]+</a>[^\\s<>]*"))
                        {
                            name = Regex.Match(strArray[2], "[^\\s<>]+?</a>[^\\s<>]*").Value;
                            name = name.Replace("</a>", string.Empty);
                        }
                        else if (Regex.IsMatch(strArray[2], "<td>[^\\s<>]+"))
                        {
                            name = Regex.Match(strArray[2], "<td>[^\\s<>]+").Value;
                            name = name.Replace("<td>", string.Empty);
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(strArray[2], "[^\\s<>]+</span>"))
                        {
                            name = Regex.Match(strArray[2], "[^\\s<>]+?</span>").Value;
                            name = name.Replace("</a>", string.Empty);
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }

                    collection[i].Add(new FestivalInfo() { Time = time, Character = character, Name = name });
                }
            }

            return collection;
        }

        private static string GetCharacterENNameFromTHBWiki(HttpClient client, string cnName)
        {
            string code = client.GetStringAsync("https://thwiki.cc/" + cnName).Result;

            if (Regex.IsMatch(code, "<td>[A-Za-z /，]+(( *\\(|（)\\S+(\\)|）))?</td>"))
            {
                string str = Regex.Match(code, "<td>[A-Za-z /，]+((\\(|（)\\S+(\\)|）))?</td>").Value;

                if (Regex.IsMatch(str, " *(\\(|（)\\S+(\\)|）)"))
                {
                    str = Regex.Replace(str, " *(\\(|（)\\S+(\\)|）)", string.Empty);
                }

                str = str[4..^5];

                if (str.Contains(" / "))
                {
                    str = str.Split(" / ")[0];
                }
                else if (str.Contains('，'))
                {
                    str = str.Split('，')[0];
                }

                return str;
            }
            else
            {
                throw new Exception();
            }
        }

        public static FestivalCollection ConvertFromTHBWiki(FestivalCollection collection)
        {
            using HttpClient client = new();

            uint progress = 0;

            for (int i = 0; i < Enum.GetNames(typeof(FestivalType)).Length; i++)
            {
                for (int j = 0; j < collection[i].Count; j++)
                {
                    collection[i][j].Time = TimeConverter.GetENFestivalTime(collection[i][j].Time);

                    if (CHARACTER_DICTIONARY.ContainsKey(((FestivalType)i, j)))
                    {
                        collection[i][j].Character = CHARACTER_DICTIONARY[((FestivalType)i, j)];
                    }
                    else if (!string.IsNullOrEmpty(collection[i][j].Character))
                    {
                        if (collection[i][j].Character.Contains(" & "))
                        {
                            string[] strArray = collection[i][j].Character.Split(" & ");

                            for (int k = 0; k < strArray.Length; k++)
                            {
                                collection[i][j].Character = GetCharacterENNameFromTHBWiki(client, strArray[k]);

                                if (k < strArray.Length - 1)
                                {
                                    collection[i][j].Character += " & ";
                                }
                            }
                        }
                        else
                        {
                            collection[i][j].Character = GetCharacterENNameFromTHBWiki(client, collection[i][j].Character);
                        }
                    }

                    if (FESTIVAL_NAME_DICTIONARY.ContainsKey(((FestivalType)i, j)))
                    {
                        collection[i][j].Name = FESTIVAL_NAME_DICTIONARY[((FestivalType)i, j)];
                    }
                    else if (!string.IsNullOrEmpty(collection[i][j].Name))
                    {
                        if (Regex.IsMatch(collection[i][j].Character, "^TH\\d+ \\((\\S| )+\\)$"))
                        {
                            collection[i][j].Name = collection[i][j].Character.Split(' ')[0] + "'s Day";
                        }
                        else
                        {
                            collection[i][j].Name = collection[i][j].Character + "'s Day";
                        }
                    }

                    progress++;

                    int count = collection.GetTotalCount();
                    ToastManager.Update("init", ((float)progress / count).ToString(),
                        $"{progress}/{count} festivals", "Initializing...");
                }
            }

            return collection;
        }
    }
}
