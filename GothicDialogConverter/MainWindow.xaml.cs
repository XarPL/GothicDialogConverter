using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GothicDialogConverter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool init;
        public MainWindow()
        {
            InitializeComponent();
            init = true;
        }
        private void InputChanged(object sender, TextChangedEventArgs e)
        {
            if (init)
            {
                string HeroID = HeroID_Input.Text;
                string NpcID = NpcID_Input.Text;
                string NpcName = NpcName_Input.Text;
                string DialogName = DialogName_Input.Text;
                string FullDialogName = "DIA_" + NpcName + "_" + DialogName;
                bool validationResult = int.TryParse(HeroVoice_Input.Text, out int HeroVoice);
                if (!validationResult)
                {
                    HeroVoice_Label.Foreground = Brushes.Red;
                }
                else
                {
                    HeroVoice_Label.Foreground = Brushes.Black;
                }
                validationResult = int.TryParse(NpcVoice_Input.Text, out int NpcVoice);
                if (!validationResult)
                {
                    NpcVoice_Label.Foreground = Brushes.Red;
                }
                else
                {
                    NpcVoice_Label.Foreground = Brushes.Black;
                }
                if (!InputCode.Text.Contains(HeroID + ": "))
                {
                    HeroID_Label.Foreground = Brushes.Red;
                }
                else
                {
                    HeroID_Label.Foreground = Brushes.Black;
                }
                if (!InputCode.Text.Contains(NpcID + ": "))
                {
                    NpcID_Label.Foreground = Brushes.Red;
                }
                else
                {
                    NpcID_Label.Foreground = Brushes.Black;
                }
                int lineCount = InputCode.LineCount;
                OutputCode.Text = "func void DIA_"+NpcName+"_"+DialogName+"_Info()\n{\n";
                int herolines = 0;
                int npclines = 0;
                for (int line = 0; line < lineCount; line++)
                {
                    string txt = InputCode.GetLineText(line);
                    txt = Regex.Replace(txt, @"\t|\n|\r", "");
                    if (txt.Contains(HeroID + ": "))
                    {
                        OutputCode.Text += "AI_Output(other, self, \"" + FullDialogName + "_" + HeroVoice.ToString("00") + "_" + herolines.ToString("00") + "\"); //" + txt.Substring(3) + "\n";
                        herolines++;
                    }
                    else if (txt.Contains(NpcID + ": "))
                    {
                        OutputCode.Text += "AI_Output(self, other, \"" + FullDialogName + "_"+ NpcVoice.ToString("00") + "_" + npclines.ToString("00") + "\"); //" + txt.Substring(3) + "\n";
                        npclines++;
                    }
                    else if (txt.Contains(NpcID + "<<" + HeroID) || txt.Contains(HeroID + ">>" + NpcID))
                    {
                        if (txt.Split(' ').Length == 3)
                        {
                            OutputCode.Text += "B_GiveInvItems (other, self, " + txt.Split(' ')[1] + ", " + txt.Split(' ')[2] + "\n";
                        }
                    }
                    else if (txt.Contains(NpcID + ">>" + HeroID) || txt.Contains(HeroID + "<<" + NpcID))
                    {
                        if (txt.Split(' ').Length == 3)
                        {
                            OutputCode.Text += "B_GiveInvItems (self, other, " + txt.Split(' ')[1] + ", " + txt.Split(' ')[2] + ");\n";
                        }
                    }
                    else if (txt.Contains(Properties.Settings.Default.EndDialogKeyword))
                    {
                        OutputCode.Text += "AI_StopProcessInfos (self);";
                    }
                }
                OutputCode.Text += "\n};";
            }
        }
        
    }
}
