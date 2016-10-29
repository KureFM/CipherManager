using CipherManager.Core;
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

namespace CipherManager.View
{
    /// <summary>
    /// AsymmetricResult.xaml 的交互逻辑
    /// </summary>
    public partial class AsymmetricResult : UserControl
    {
        private AsymmetricCipher cipher;

        public AsymmetricCipher Cipher
        {
            get
            {
                AsymmetricCipher arg_18_0;
                if ((arg_18_0 = this.cipher) == null)
                {
                    arg_18_0 = (this.cipher = new AsymmetricCipher());
                }
                return arg_18_0;
            }
            set
            {
                if (value != null)
                {
                    this.cipher = value;
                    this.privateCipherBox.Data = this.cipher.PrivateKey.Content;
                    this.publicCipherBox.Data = this.cipher.PublicKey.Content;
                }
            }
        }

        public AsymmetricResult()
        {
            InitializeComponent();
        }
    }
}
