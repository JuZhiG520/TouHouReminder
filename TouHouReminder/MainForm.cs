namespace TouHouReminder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            FestivalCollection? collection = ConfigLoader.Load();

            if (collection is null)
            {
                Environment.Exit(1);
            }

            List<(int, int, TimeConverter.MatchResult)> indexList = new();

            int maximum = Enum.GetNames(typeof(FestivalType)).Length - 1;
            for (int i = 0; i <= maximum; i++)
            {
                for (int j = 0; j < collection[i].Count; j++)
                {
                    try
                    {
                        if (TimeConverter.CompareTimeWithSpanString(DateTime.Now, collection[i][j].Time,
                            out TimeConverter.MatchResult matchResult))
                        {
                            indexList.Add((i, j, matchResult));
                        }
                    }
                    catch
                    {
                        ToastManager.Send("Try to restart the application",
                            "There is something wrong with the config file");

                        Environment.Exit(1);
                    }
                }
            }

            for (int i = 0; i < indexList.Count; i++)
            {
                if (!indexList[i].Item3.IsCompletelyMatched)
                {
                    FestivalInfo target = collection[indexList[i].Item1][indexList[i].Item2];

                    try
                    {
                        if (target.Name.StartsWith("The"))
                        {
                            target.Name = "the" + target.Name[3..];
                        }

                        ToastManager.Send($"Now is {target.Name}", $"{target.Time} is the TouHou time for {target.Character}!",
                            DateTime.Today.AddHours(indexList[i].Item3.Hour).AddMinutes(indexList[i].Item3.Minute));
                    }
                    catch
                    {
                        if (target.Name.StartsWith("the"))
                        {
                            target.Name = "The" + target.Name[3..];
                        }

                        ToastManager.Send($"{target.Name} has passed", $"{target.Time} is the TouHou time for {target.Character}!");
                    }

                    indexList.RemoveAt(i);
                }
            }

            if (indexList.Count == 0)
            {
                Environment.Exit(0);
            }

            for (int i = 0; i < Enum.GetNames(typeof(FestivalType)).Length; i++)
            {
                if (indexList.FindIndex(item => item.Item1 == i) != -1)
                {
                    if (i < (int)FestivalType.Months)
                    {
                        indexList.RemoveAll(item => collection[item.Item1][item.Item2].Time.StartsWith("Every day in")
                        || collection[item.Item1][item.Item2].Time.Contains("to"));
                    }

                    indexList.RemoveAll(item => item.Item1 > i);
                    break;
                }
            }

            List<string> timeList = new(), charList = new(), nameList = new();
            indexList.ForEach(item =>
            {
                FestivalInfo info = collection[item.Item1][item.Item2];

                if (!timeList.Contains(info.Time))
                {
                    timeList.Add(collection[item.Item1][item.Item2].Time);
                }

                if (!charList.Contains(info.Character))
                {
                    charList.Add(collection[item.Item1][item.Item2].Character);
                }

                if (!nameList.Contains(info.Name))
                {
                    nameList.Add(collection[item.Item1][item.Item2].Name);
                }
            });

            FestivalInfo info = new();

            for (int i = 0; i < timeList.Count; i++)
            {
                info.Time += timeList[i];

                if (i < indexList.Count - 2)
                {
                    info.Time += ", ";
                }
                else if (i == indexList.Count - 2)
                {
                    info.Time += " and ";
                }
            }

            for (int i = 0; i < charList.Count; i++)
            {
                info.Character += charList[i];

                if (i < indexList.Count - 2)
                {
                    info.Character += ", ";
                }
                else if (i == indexList.Count - 2)
                {
                    info.Character += " and ";
                }
            }

            for (int i = 0; i < nameList.Count; i++)
            {
                if (nameList[i].StartsWith("The"))
                {
                    nameList[i] = "the" + nameList[i][3..];
                }

                info.Name += nameList[i];

                if (i < indexList.Count - 2)
                {
                    info.Name += ", ";
                }
                else if (i == indexList.Count - 2)
                {
                    info.Name += " and ";
                }
            }

            ToastManager.Send($"Today is {info.Name}", $"{info.Time} is the TouHou day for {info.Character}!");

            Environment.Exit(0);

            //InitializeComponent();
        }
    }
}