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
using System.Security.Cryptography;
using Account.Class;
using DateModel;
using System.Windows.Shapes;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_change_password.xaml
    /// </summary>
    public partial class win_change_password : Window
    {
        AccountEntities db = new AccountEntities();
        public win_change_password()
        {
            InitializeComponent();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private Boolean Checkable()
        {
            if (pwd_old.Password == string.Empty)
            {
                MessageBox.Show("لطفا رمز قدیمی خود را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                pwd_old.Focus();
                return false;
            }
            if (pwd_new.Password == string.Empty)
            {
                MessageBox.Show("لطفا رمز جدید خود را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                pwd_new.Focus();
                return false;
            }
            if (pwd_verify.Password == string.Empty)
            {
                MessageBox.Show("لطفا رمز جدید خود را دوباره وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                pwd_verify.Focus();
                return false;
            }
            return true;
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if (!Checkable())
                return;
            else
            {
                string pass = Hasher(pwd_old.Password);
                string newpass = Hasher(pwd_new.Password);
                var query = from p in db.USERS where p.USERID == PublicVar.GlobalUserId select p;
                var run = query.ToList();
                if (run[0].USERPASSWORD != pass)
                {
                    MessageBox.Show("رمز وارد شده صحیح نمی باشد", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    return;
                }
                if(pwd_new.Password==pwd_verify.Password)
                {
                    db.SP_USERS_PASS_CHANGE(PublicVar.GlobalUserId, newpass);
                    db.SaveChanges();
                    MessageBox.Show("رمز شما با موفقیت تغییر یافت", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    this.Close();
                }
                else
                    MessageBox.Show("لطفا رمز جدید خود را صحیح وارد نمایید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            }
        }
        private string Hasher(string input)
        {
            SHA256CryptoServiceProvider sHA256 = new SHA256CryptoServiceProvider();
            byte[] a = UTF8Encoding.UTF8.GetBytes(input);
            byte[] b = sHA256.ComputeHash(a);
            string Hashpass = BitConverter.ToString(b);
            return Hashpass;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pwd_old.Focus();
        }
    }
}
