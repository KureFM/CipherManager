using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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
    /// ByteBox.xaml 的交互逻辑
    /// </summary>
    public partial class ByteBox : UserControl, INotifyPropertyChanged
    {
        private string dataString;

        private string split = "";

        private byte[] data;

        private bool isUpper;

        private bool signed;

        public event PropertyChangedEventHandler PropertyChanged;


        public bool IsUpper
        {
            get
            {
                return this.isUpper;
            }
            set
            {
                if (this.isUpper != value)
                {
                    this.isUpper = value;
                    this.DataString = this.GetHexString(this.IsUpper, this.Split);
                }
            }
        }

        public bool Signed
        {
            get
            {
                return this.signed;
            }
            set
            {
                if (this.signed != value)
                {
                    this.signed = value;
                    this.DataString = this.GetInteger(!this.Signed).ToString();
                }
            }
        }

        public string Split
        {
            get
            {
                return this.split;
            }
            set
            {
                if (this.split != value)
                {
                    this.split = value;
                    this.DataString = this.GetHexString(this.IsUpper, this.Split);
                }
            }
        }

        public byte[] Data
        {
            get
            {
                return data ?? (data = new byte[0]);
            }
            set
            {
                if (value == null)
                {
                    this.data = new byte[0];
                    return;
                }
                if (this.data == null || !this.data.SequenceEqual(value))
                {
                    this.data = value;
                    this.SelectChange();
                }
            }
        }

        public ByteDisplayType Display
        {
            get;
            set;
        }

        public string DataString
        {
            get
            {
                return this.dataString;
            }
            set
            {
                if (value != this.dataString)
                {
                    this.dataString = value;
                    this.OnPropertyChanged("DataString");
                }
            }
        }

        public ByteBox()
        {
            this.Display = ByteDisplayType.Hex;
            this.InitializeComponent();
            base.DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.SelectChange();
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SelectChange()
        {
            switch (this.dspSelect.SelectedIndex)
            {
                case 0:
                    this.DataString = this.GetHexString(this.IsUpper, this.Split);
                    return;
                case 1:
                    this.DataString = Convert.ToBase64String(this.Data);
                    return;
                case 2:
                    this.DataString = this.GetInteger(!this.Signed).ToString();
                    return;
                default:
                    return;
            }
        }

        public BigInteger GetInteger(bool ignoreSign = true)
        {
            byte[] array;
            if (ignoreSign)
            {
                array = new byte[this.Data.Length + 1];
                Buffer.BlockCopy(this.Data, 0, array, 1, this.Data.Length);
                Array.Reverse(array);
                array[array.Length - 1] = 0;
            }
            else
            {
                array = new byte[this.Data.Length];
                Buffer.BlockCopy(this.Data, 0, array, 0, this.Data.Length);
                Array.Reverse(array);
            }
            return new BigInteger(array);
        }

        public string GetHexString(bool isupper, string split)
        {
            if (this.Data.Length <= 0)
            {
                return "";
            }
            StringBuilder stringBuilder = new StringBuilder();
            string format = isupper ? "X2" : "x2";
            byte[] array = this.Data;
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                stringBuilder.Append(b.ToString(format));
                stringBuilder.Append(split);
            }
            stringBuilder.Length -= split.Length;
            return stringBuilder.ToString();
        }

        private void dspSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectChange();
        }
    }
}
