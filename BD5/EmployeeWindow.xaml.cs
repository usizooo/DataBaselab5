using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BD5
{
    public partial class EmployeeWindow : Window
    {
        private Dictionary<string, Page> pages = new Dictionary<string, Page>();
        public EmployeeWindow(List<TabInfo> tabs)
        {
            InitializeComponent();

            for (int i = 0; i < tabs.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                if (!pages.ContainsKey(tabs[i].TableName))
                {
                    pages.Add(tabs[i].TableName, tabs[i].Page);
                }
                TextBox textBox = new TextBox();
                textBox.Text = tabs[i].TableName;
                textBox.IsReadOnly = true;
                textBox.FontFamily = new FontFamily("Cascadia Code SemiBold");
                textBox.TextAlignment = TextAlignment.Center;
                textBox.Margin = new Thickness(5);
                textBox.PreviewMouseDown += TextBox_PreviewMouseDown;
                Grid.SetRow(textBox, 0);
                Grid.SetColumn(textBox, i);
                grid.Children.Add(textBox);
            }

            Grid.SetRow(frame, 1);
            Grid.SetColumnSpan(frame, tabs.Count);

            if (pages.Count > 0)
            {
                frame.Content = pages.First().Value;
            }
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null && sender is TextBox)
            {
                TextBox textBox = sender as TextBox;
                if (pages.ContainsKey(textBox.Text))
                {
                    frame.Content = pages[textBox.Text];
                }
            }
        }
    }
}
