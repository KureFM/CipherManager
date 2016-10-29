using CipherManager.Core;
using CipherManager.ViewModel;
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
    /// GeneratorPanel.xaml 的交互逻辑
    /// </summary>
    public partial class GeneratorPanel : UserControl
    {
        private const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private const string lower = "abcderghijklmnopqrstuvwxyz";

        private const string number = "0123456789";

        public int GeneratorIndex
        {
            get;
            set;
        }

        public int LengthIndex
        {
            get;
            set;
        }

        public bool IsUpper
        {
            get;
            set;
        }

        public bool IsLower
        {
            get;
            set;
        }

        public bool IsNumber
        {
            get;
            set;
        }

        public bool IsOther
        {
            get;
            set;
        }

        public string OtherStr
        {
            get;
            set;
        }

        public List<GeneratorViewModel> GeneratorList
        {
            get;
            set;
        }

        public GeneratorPanel()
        {
            this.GeneratorIndex = 0;
            this.LengthIndex = 0;
            this.IsUpper = true;
            this.IsLower = true;
            this.IsNumber = true;
            this.IsOther = false;
            this.OtherStr = "";
            this.GeneratorList = new List<GeneratorViewModel>
            {
                new GeneratorViewModel
                {
                    GeneratorName = "CSRNG",
                    Generator = SymmetricCipherGenerator.CSRNG,
                    LengthName = "Bit Length",
                    ResultView = new SymmetricResult()
                },
                new GeneratorViewModel
                {
                    GeneratorName = "Big Prime",
                    Generator = SymmetricCipherGenerator.BigPrime,
                    LengthName = "Bit Length",
                    ResultView = new SymmetricResult(ByteDisplayType.Integer)
                },
                new GeneratorViewModel
                {
                    GeneratorName = "RSA",
                    Generator = AsymmetricCipherGenerator.RSA,
                    LengthName = "Bit Length",
                    ResultView = new AsymmetricResult()
                },
                new GeneratorViewModel
                {
                    GeneratorName = "DSA",
                    Generator = AsymmetricCipherGenerator.DSA,
                    LengthName = "Bit Length",
                    ResultView = new AsymmetricResult()
                },
                new GeneratorViewModel
                {
                    GeneratorName = "Password",
                    Generator = new PasswordGenerator(),
                    LengthName = "String Length",
                    ResultView = new PasswordResult()
                }
            };
            this.InitializeComponent();
            base.DataContext = this;
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            object generator = this.GeneratorList[this.GeneratorIndex].Generator;
            if (generator is SymmetricCipherGenerator)
            {
                this.GenerateSymmetricCipher((SymmetricCipherGenerator)generator, this.LengthIndex);
                return;
            }
            if (generator is AsymmetricCipherGenerator)
            {
                this.GenerateAsymmetricCipher((AsymmetricCipherGenerator)generator, this.LengthIndex);
                return;
            }
            if (generator is PasswordGenerator)
            {
                this.GeneratePassword((PasswordGenerator)generator, this.LengthIndex);
                return;
            }
            MessageBox.Show("UnKnow Type!");
        }

        private void GenerateSymmetricCipher(SymmetricCipherGenerator scg, int lenIndex)
        {
            int cipherSize = scg.CipherSize[lenIndex];
            SymmetricCipher cipher = scg.Generate(cipherSize);
            SymmetricResult symmetricResult = (SymmetricResult)this.resultContentControl.Content;
            symmetricResult.Cipher = cipher;
        }

        private void GenerateAsymmetricCipher(AsymmetricCipherGenerator acg, int lenIndex)
        {
            int cipherSize = acg.CipherSize[lenIndex];
            AsymmetricCipher cipher = acg.Generate(cipherSize);
            AsymmetricResult asymmetricResult = (AsymmetricResult)this.resultContentControl.Content;
            asymmetricResult.Cipher = cipher;
        }

        private void GeneratePassword(PasswordGenerator pg, int lenIndex)
        {
            int cipherLen = pg.CipherSize[lenIndex];
            string data = pg.Generate(cipherLen, this.AcceptString(), "");
            PasswordResult passwordResult = (PasswordResult)this.resultContentControl.Content;
            passwordResult.Data = data;
        }

        private string AcceptString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.IsUpper)
            {
                stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            }
            if (this.IsLower)
            {
                stringBuilder.Append("abcderghijklmnopqrstuvwxyz");
            }
            if (this.IsNumber)
            {
                stringBuilder.Append("0123456789");
            }
            if (this.IsOther)
            {
                stringBuilder.Append(this.OtherStr);
            }
            return stringBuilder.ToString();
        }
    }
}
