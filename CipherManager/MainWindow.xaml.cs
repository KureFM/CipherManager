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
using MahApps.Metro.Controls;
using CipherManager.ViewModel;
using CipherManager.View;
using CipherManager.Core;

namespace CipherManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public List<FunctionPanelViewModel> FuncList
        {
            get;
            set;
        }

        public MainWindow()
        {
            this.FuncList = new List<FunctionPanelViewModel>
            {
                new FunctionPanelViewModel
                {
                    FunctionName = "File Info",
                    Panel = new FileInfoPanel()
                },
                new FunctionPanelViewModel
                {
                    FunctionName = "Generator",
                    Panel = new GeneratorPanel()
                },
                new FunctionPanelViewModel
                {
                    FunctionName = "Manager",
                    Panel = new FileInfoPanel()
                },
                new FunctionPanelViewModel
                {
                    FunctionName = "Security",
                    Panel = new FileInfoPanel()
                },
                new FunctionPanelViewModel
                {
                    FunctionName = "Setting",
                    Panel = new FileInfoPanel()
                },
                new FunctionPanelViewModel
                {
                    FunctionName = "About",
                    Panel = new FileInfoPanel()
                }
            };
            this.InitializeComponent();
            FileManager.GetInstance();
            base.DataContext = this;
        }
    }
}
