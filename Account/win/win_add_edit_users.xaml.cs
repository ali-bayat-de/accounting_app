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
using System.Text.RegularExpressions;
using System.Windows.Shapes;
using DateModel;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_add_edit_users.xaml
    /// </summary>
    public partial class win_add_edit_users : Window
    {
        AccountEntities db = new AccountEntities();
        public byte Type { set; get; }
        public int ID { set; get; }
        public win_add_edit_users()
        {
            InitializeComponent();
        }
        private Boolean Checkable()
        {
            if(string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("لطفا نام کاربر را وارد کنید","هشدار",MessageBoxButton.OK,MessageBoxImage.Warning,MessageBoxResult.OK,MessageBoxOptions.RightAlign);
                txt_name.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_family.Text))
            {
                MessageBox.Show("لطفا نام خانوادگی کاربر را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_family.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_username.Text))
            {
                MessageBox.Show("لطفا نام کاربری را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_username.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_address.Text))
            {
                MessageBox.Show("لطفا آدرس کاربر را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_address.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(password.Password))
            {
                MessageBox.Show("لطفا رمز عبور کاربر را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                password.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_no.Text))
            {
                MessageBox.Show("لطفا تلفن کاربر را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_no.Focus();
                return false;
            }
            return true;
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (!Checkable())
                return;
            else
            {
                try
                {
                    if(Type==1)
                    {
                        db.SP_INSERT_USERS(txt_name.Text.Trim(), txt_family.Text.Trim(), Convert.ToByte(cmb_age.SelectedValue), Convert.ToByte(cmb_sex.SelectedIndex), txt_username.Text.Trim(), HashPass(password.Password.Trim()), txt_address.Text.Trim(), string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)), txt_no.Text.Trim());
                        db.SaveChanges();
                        MessageBox.Show("کاربر مورد نظر با موفقیت ثبت شد", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    }
                    else if(Type==2)
                    {
                        USER U = (from p in db.USERS where ID==p.USERID select p).SingleOrDefault();
                        U.USERADDRESS = txt_address.Text.Trim();
                        U.USERAGE = Convert.ToByte(cmb_age.SelectedValue);
                        U.USERFAMILY = txt_family.Text.Trim();
                        U.USERNAME = txt_name.Text.Trim();
                        U.USERNO = txt_no.Text.Trim();
                        U.USERSEX = Convert.ToByte(cmb_sex.SelectedIndex);
                        U.USERUPDATEDATE = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text));
                        db.SaveChanges();
                        MessageBox.Show("کاربر مورد نظر با موفقیت بروزرسانی شد", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطایی هنگام ذخیره اطلاعات به وجود آمد، لطفا دوباره امتحان نمایید." +"\n"+ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                }
                finally
                {
                    this.Close();
                }
                
            }
        }
        private String HashPass(string input)
        {
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] a = UTF8Encoding.UTF8.GetBytes(input);
            byte[] b = sha256.ComputeHash(a);
            string result = BitConverter.ToString(b);
            return result;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for(int i=20;i<50;i++)
            {
                cmb_age.Items.Add(i);
            }
            cmb_age.SelectedIndex = 5;
            cmb_sex.Items.Add("موارد دیگر");
            cmb_sex.Items.Add("مرد");
            cmb_sex.Items.Add("زن");
            cmb_sex.SelectedIndex = 2;
            txt_name.Focus();
            if(Type==2)
            {
                var query = from u in db.USERS where ID==u.USERID select u;
                var run = query.ToList();
                
                
                    lbl_title.Content = "فرم ویرایش کاربران";
                    txt_username.IsEnabled = false;
                    password.IsEnabled = false;
                    password.Password = "************";
                    txt_username.Text = run[0].USERUSERNAME;
                    txt_no.Text = run[0].USERNO;
                    txt_name.Text = run[0].USERNAME;
                    txt_family.Text = run[0].USERFAMILY;
                    txt_address.Text = run[0].USERADDRESS;
                    cmb_age.SelectedIndex = Convert.ToByte(run[0].USERAGE-20);
                    cmb_sex.SelectedIndex = Convert.ToByte(run[0].USERSEX);
                
            }
        }

        private void txt_no_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }
    }
}
