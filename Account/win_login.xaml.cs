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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Security.Cryptography;
using Account.Class;
using DateModel;
using System.Net;

namespace Account
{
    /// <summary>
    /// Interaction logic for win_login.xaml
    /// </summary>
    public partial class win_login : Window
    {
        AccountEntities db = new AccountEntities();
        public win_login()
        {
            InitializeComponent();
        }
        private void SetDate()
        {
            lbl_dayofweek.Content = calender.DisplayDate.PersianDayOfWeek;
            lbl_dayofmonth.Content = calender.DisplayDate.Day;
            lbl_month.Content = calender.DisplayDate.MonthAsPersianMonth;
            lbl_year.Content = calender.DisplayDate.Year;
            txt_username.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetDate();
            Get_Register();
            
            password.Focus();
        }
        
        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            Set_Register();
           // win_main m = new win_main(); m.ShowDialog();
        }
        private string HashPass(string input)
        {
            SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider();
            Byte[] first = UTF8Encoding.UTF8.GetBytes(input);
            Byte[] second = sha.ComputeHash(first);
            string result = BitConverter.ToString(second);
            return result;
        }
        private void Set_Register()
        {
            if (string.IsNullOrWhiteSpace(txt_username.Text))
            {
                MessageBox.Show("لطفا نام کاربری را وارد کنید");
                txt_username.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(password.Password))
            {
                MessageBox.Show("لطفا رمز عبور را وارد کنید");
                password.Focus();
                return;
            }
            string pass = HashPass(password.Password.Trim());
            var query = from u in db.USERS where u.USERUSERNAME == txt_username.Text.Trim() where u.USERPASSWORD == pass select u;
            var run = query.ToList();
            if (run.Count ==1)
            {
                PublicVar.GlobalUserId = run[0].USERID;
                PublicVar.GlobalForName = run[0].USERNAME;
                PublicVar.GlobalSurName = run[0].USERFAMILY;
                win_main main = new win_main();
                RegistryKey Reg = Registry.CurrentUser.CreateSubKey("SoftWare\\Account");
                try
                {

                    Reg.SetValue("SoftWare\\Account", txt_username.Text.Trim());
                    Get_UserInfo();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Reg.Close();
                }
                this.Hide();
                main.ShowDialog();
            }
            else
            {
                MessageBox.Show("نام کاربری یا رمز عبور صحیح نیست");
                password.Focus();
            }
            
        }
        private void Get_UserInfo()
        {
            string PCName = System.Environment.MachineName;
            IPHostEntry hostEntry = Dns.GetHostByName(PCName);
            IPAddress[] iP= hostEntry.AddressList;
            string IPAddress = iP[0].ToString();
            USERSLOG UL = new USERSLOG();
            UL.IPADDRESS = IPAddress;
            UL.PCNAME = PCName;
            UL.STARTDATE = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)) + " - "+string.Format("{0:HH:mm:ss}",DateTime.Now);
            UL.USERSID = PublicVar.GlobalUserId;
            db.USERSLOGs.Add(UL);
            db.SaveChanges();
        }
        private void Get_Register()
        {
            RegistryKey Reg = Registry.CurrentUser.CreateSubKey("SoftWare\\Account");
            try
            {

                txt_username.Text = (string)Reg.GetValue("SoftWare\\Account");
            }
            catch
            {
                throw;
            }
            finally
            {
                Reg.Close();
            }
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
