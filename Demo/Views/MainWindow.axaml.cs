using Avalonia;
using Avalonia.Controls;
using Avalonia.DeferredStartup;
using Avalonia.Markup.Xaml;

namespace Demo.Views
{
    public partial class MainWindow : DeferredWindow
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
