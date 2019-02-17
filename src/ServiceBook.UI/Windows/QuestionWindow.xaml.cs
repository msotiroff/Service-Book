using System.Windows;

namespace ServiceBook.UI.Windows
{
    /// <summary>
    /// Interaction logic for QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        public QuestionWindow(string message)
        {
            InitializeComponent();

            this.txtMessage.Text = message;
        }

        public bool Confirmed { get; private set; }

        private void ButtonNo_Clicked(object sender, RoutedEventArgs e)
        {
            this.Confirmed = false;
            this.Close();
        }

        private void ButtonYes_Clicked(object sender, RoutedEventArgs e)
        {
            this.Confirmed = true;
            this.Close();
        }
    }
}
