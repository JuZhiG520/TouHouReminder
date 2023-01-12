using Microsoft.Win32;
using System.Reflection;
using System.Security.AccessControl;

namespace TouHouReminder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            FestivalCollection? collection = ConfigLoader.Load($"{Application.StartupPath}config.json");

            if (collection is null)
            {
                throw new Exception();
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

                        throw;
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

            if (indexList.Count > 0)
            {
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
            }

            bool autoExit = GetRegistryKey_AutoExit();
            if (autoExit && !Environment.CurrentDirectory.Equals(Application.StartupPath[..^1]))
            {
                Environment.Exit(0);
            }


            InitializeComponent();
            ToolStripMenuItem_AutoRun.Checked = TryGetValueInRegistry_AutoRun();
            ToolStripMenuItem_AutoExit.Checked = autoExit;

            List<TreeNode> treeNodeArray = new();
            for (int i = 0; i < Enum.GetNames(typeof(FestivalType)).Length; i++)
            {
                treeNodeArray.Add(TreeView.Nodes.Add(((FestivalType)i).ToString()));
                treeNodeArray[i].Nodes.AddRange((from item in collection[i] select new TreeNode(item.Name)).ToArray());

                for (int j = 0; j < treeNodeArray[i].Nodes.Count; j++)
                {
                    treeNodeArray[i].Nodes[j].Nodes.Add($"Character: {collection[i][j].Character}");
                    treeNodeArray[i].Nodes[j].Nodes.Add($"Time: {collection[i][j].Time}");
                }
            }
        }

        private static bool TryGetValueInRegistry_AutoRun()
        {
            string valueName = Application.ProductName;
            string subKeyName = Environment.Is64BitProcess ?
                 "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"
                 : "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";

            RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey(subKeyName,
                RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);

            if (registryKey is not null)
            {
                return registryKey.GetValue(valueName) is not null;
            }

            return false;
        }

        private static void SetRegistryKey_AutoRun(bool autoRun)
        {
            string valueName = Application.ProductName;
            string value = $"\"{Application.ExecutablePath}\"";
            string subKeyName = Environment.Is64BitProcess ?
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"
                : "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";

            RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey(subKeyName,
                RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);

            if (autoRun)
            {
                registryKey ??= Registry.LocalMachine.CreateSubKey(subKeyName);

                if (registryKey.GetValue(valueName) is null)
                {
                    registryKey.SetValue(valueName, value);
                }
                else
                {
                    registryKey.DeleteValue(valueName);
                    registryKey.SetValue(valueName, value);
                }
            }
            else
            {
                if (registryKey is not null && registryKey.GetValue(valueName) is not null)
                {
                    registryKey.DeleteValue(valueName);
                }
            }
        }

        private static bool GetRegistryKey_AutoExit()
        {
            string subKeyName = $"SOFTWARE\\{Application.ProductName}";

            RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey(subKeyName,
                RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);

            if (registryKey is not null)
            {
                object? value = registryKey.GetValue("AutoExit");
                if (value is not null)
                {
                    return value.Equals(true.ToString());
                }
            }

            return false;
        }

        private static void SetRegistryKey_AutoExit(bool autoExit)
        {
            string subKeyName = $"SOFTWARE\\{Application.ProductName}";

            RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey(subKeyName,
                RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);

            if (autoExit)
            {
                registryKey ??= Registry.LocalMachine.CreateSubKey(subKeyName);

                if (registryKey.GetValue("AutoExit") is null)
                {
                    registryKey.SetValue("AutoExit", autoExit);
                }
                else
                {
                    registryKey.DeleteValue("AutoExit");
                    registryKey.SetValue("AutoExit", autoExit);
                }
            }
            else
            {
                if (registryKey is not null && registryKey.GetValue(subKeyName) is not null)
                {
                    registryKey.DeleteValue("AutoExit");
                }
            }
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip.Show();
            }
        }

        private void ToolStripMenuItem_ShowTimetable_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void ToolStripMenuItem_AutoRun_Click(object sender, EventArgs e)
        {
            SetRegistryKey_AutoRun(ToolStripMenuItem_AutoRun.Checked);
        }

        private void ToolStripMenuItem_AutoExit_Click(object sender, EventArgs e)
        {
            SetRegistryKey_AutoExit(ToolStripMenuItem_AutoExit.Checked);
        }

        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}