using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Pilkarze
{
    public partial class MainWindow : System.Windows.Window
    {
        private System.Collections.Generic.List<Player> team = new System.Collections.Generic.List<Player>();
        private readonly int minAge = 15;
        private readonly int maxAge = 55;
        private readonly string defaultName = "Podaj imię";
        private readonly string defaultLastName = "Podaj nazwisko";

        private readonly string teamDataFile = System.IO.Path.Combine(System.Environment.CurrentDirectory, @"team.txt");

        public MainWindow()
        {
            InitializeComponent();
            InitializeComboBox();
            CheckSaveFile();
            LoadTeam();
        }

        #region onLoad
        private void InitializeComboBox()
        {
            for (int i = minAge; i <= maxAge; i++)
                Age_cb.Items.Add(i);
        }

        private void CheckSaveFile()
        {
            if (!System.IO.File.Exists(teamDataFile))
            {
                System.IO.FileStream fs = System.IO.File.Create(teamDataFile);
                fs.Close();
            }
        }

        private void LoadTeam()
        {
            string[] playersData = System.IO.File.ReadAllLines(teamDataFile);
            if (playersData.Length < 4)
                AddDefaultTeam();
            else
                LoadTeamFromData(ref playersData);
            UpdateTeam();
        }

        private void AddDefaultTeam()
        {
            team.Add(new Player("Paweł", "Piłat", 20, 70));
            team.Add(new Player("Rober", "Prawondowski", 40, 105.5));
            team.Add(new Player("Zbigniew", "Stonoga", 55, 80));
        }

        private void LoadTeamFromData(ref string[] data)
        {
            for (int i = 0; i < data.Length; i += 4)
            {
                string name = data[i];
                string lastName = data[i + 1];
                bool isAgeCorrect = System.Int32.TryParse(data[i + 2], out int age);
                bool isWeightCorrect = System.Double.TryParse(data[i + 3], out double weight);
                if (name != System.String.Empty && lastName != System.String.Empty && isAgeCorrect && isWeightCorrect)
                    team.Add(new Player(name, lastName, age, weight));
            }
        }
        #endregion

        #region TextBox
        #region NameTextBox
        private void Name_tb_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Text == defaultName)
                ClearTextBox(ref tb);
        }

        private void Name_tb_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Text == System.String.Empty)
                ResetTextBox(ref tb, defaultName);
        }
        #endregion

        #region LastNameTextBox
        private void LastName_tb_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Text == defaultLastName)
                ClearTextBox(ref tb);
        }

        private void LastName_tb_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Text == System.String.Empty)
                ResetTextBox(ref tb, defaultLastName);
        }
        #endregion

        private void ClearTextBox(ref System.Windows.Controls.TextBox textBox)
        {
            textBox.Text = System.String.Empty;
            textBox.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void ResetTextBox(ref System.Windows.Controls.TextBox textBox, string defaultValue)
        {
            textBox.Text = defaultValue;
            textBox.Foreground = System.Windows.Media.Brushes.Gray;
        }

        private void SetTextBox(ref System.Windows.Controls.TextBox textBox, string text)
        {
            textBox.Text = text;
            textBox.Foreground = System.Windows.Media.Brushes.Black;
        }
        #endregion

        #region Slider
        private void Weight_s_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (Weight_s.IsLoaded)
                Weight_l.Content = e.NewValue.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("pl-PL")) + " kg";
        }

        private void ResetSlider(ref System.Windows.Controls.Slider slider)
        {
            slider.Value = slider.Minimum;
        }

        private void SetSlider(ref System.Windows.Controls.Slider slider, double value)
        {
            if (slider.Minimum > value || value > slider.Maximum) return;
            slider.Value = value;
        }
        #endregion

        #region ComboBox
        private void ResetComboBox(ref System.Windows.Controls.ComboBox comboBox)
        {
            comboBox.SelectedIndex = 0;
        }

        private void SetComboBox(ref System.Windows.Controls.ComboBox comboBox, int index)
        {
            if (index < 0 || index > comboBox.Items.Count) return;
            comboBox.SelectedIndex = index;
        }
        #endregion

        #region Button
        private void Add_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Name_tb.Text == System.String.Empty || Name_tb.Text == defaultName)
            {
                Name_tb.BorderThickness = new System.Windows.Thickness(3, 3, 3, 3);
                Name_tb.BorderBrush = System.Windows.Media.Brushes.Red;
                Name_tb.ToolTip = "Wprowadź imię!";
                return;
            }
            if (LastName_tb.Text == System.String.Empty || LastName_tb.Text == defaultLastName)
            {
                LastName_tb.BorderThickness = new System.Windows.Thickness(3, 3, 3, 3);
                LastName_tb.BorderBrush = System.Windows.Media.Brushes.Red;
                LastName_tb.ToolTip = "Wprowadź nazwisko!";
                return;
            }
            AddNewPlayer(Name_tb.Text, LastName_tb.Text, Age_cb.SelectedIndex + minAge, Weight_s.Value);
            UpdateTeam();
            ResetControls();
        }

        private void Modify_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            team[Team_lb.SelectedIndex].Update(Name_tb.Text, LastName_tb.Text, minAge + Age_cb.SelectedIndex, Weight_s.Value);
            Modify_btn.IsEnabled = false;
            UpdateTeam();
            ResetControls();
        }

        private void Delete_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DeletePlayer(Team_lb.SelectedIndex);
            UpdateTeam();
            ResetControls();
        }
        #endregion

        #region ButtonsLogic
        private void AddNewPlayer(string name, string lastName, int age, double weight)
        {
            team.Add(new Player(name, lastName, age, weight));
        }

        private void SetPlayerInfoToControls(Player player)
        {
            SetTextBox(ref Name_tb, player.Name);
            SetTextBox(ref LastName_tb, player.LastName);
            SetComboBox(ref Age_cb, player.Age - minAge);
            SetSlider(ref Weight_s, player.Weight);
        }

        private void DeletePlayer(int index)
        {
            team.RemoveAt(index);
            for (int i = index + 1; i < team.Count; i++)
            {
                team[i - 1] = team[i];
            }
        }
        #endregion

        private void UpdateTeam()
        {
            Team_lb.ItemsSource = null;
            Team_lb.ItemsSource = team;
        }

        private void ResetControls()
        {
            ResetTextBox(ref Name_tb, defaultName);
            ResetTextBox(ref LastName_tb, defaultLastName);
            ResetSlider(ref Weight_s);
            ResetComboBox(ref Age_cb);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(teamDataFile))
            {
                foreach (Player player in team)
                {
                    file.WriteLine(player.Name);
                    file.WriteLine(player.LastName);
                    file.WriteLine(player.Age);
                    file.WriteLine(player.Weight);
                }
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = Team_lb.SelectedIndex;
            if (index == -1) return;
            SetPlayerInfoToControls(team[index]);
            Modify_btn.IsEnabled = true;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            tb.BorderThickness = new System.Windows.Thickness(1, 1, 1, 1);
            tb.BorderBrush = System.Windows.Media.Brushes.Black;
            tb.ToolTip = null;
        }
    }
}
