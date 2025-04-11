using System;
using System.Windows;
using KeyboardLayoutSwitcherQToF.Services;


namespace KeyboardLayoutSwitcherQToF
{
    public partial class MainWindow : Window
    {
        private readonly IKeyboardLayoutService _keyboardService;
        private bool _isProgrammaticCheck = false; // DetectAndSelectActiveLayout sonucunda Event handler'ları bypass etmek için

        public MainWindow()
        {
            InitializeComponent();

            _keyboardService = new KeyboardLayoutService();

            DetectAndSelectActiveLayout();
        }

        private void SetKeyboardLayout(string layoutHex)
        {
            try
            {
                _keyboardService.SetKeyboardLayout(layoutHex);
                MessageBox.Show("Keyboard layout changed successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RadioButton_Q_Checked(object sender, RoutedEventArgs e)
        {
            if (_isProgrammaticCheck)
                return;

            SetKeyboardLayout("0000041F"); // Turkish Q
        }

        private void RadioButton_F_Checked(object sender, RoutedEventArgs e)
        {
            if (_isProgrammaticCheck)
                return;

            SetKeyboardLayout("0001041F"); // Turkish F
        }

        private void DetectAndSelectActiveLayout()
        {
            _isProgrammaticCheck = true;

            string layoutId = _keyboardService.DetectActiveLayout();

            if (layoutId == "0001041F" || layoutId == "F014041F")
                RadioButton_F.IsChecked = true;
            else if (layoutId == "0000041F" || layoutId == "041F041F")
                RadioButton_Q.IsChecked = true;
            else
                MessageBox.Show($"Unknown keyboard layout: {layoutId}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            _isProgrammaticCheck = false;
        }
    }
}