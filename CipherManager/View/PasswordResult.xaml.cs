using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace CipherManager.View
{
    /// <summary>
    /// PasswordResult.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordResult : UserControl, INotifyPropertyChanged
    {
        private string data = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Data
        {
            get
            {
                return this.data;
            }
            set
            {
                if (value != this.data)
                {
                    this.data = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public PasswordResult()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
