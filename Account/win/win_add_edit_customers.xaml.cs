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
using DateModel;
using Account.Class;
using System.Text.RegularExpressions;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_add_edit_customers.xaml
    /// </summary>
    public partial class win_add_edit_customers : Window
    {
        AccountEntities db = new AccountEntities();
        public bool Type { set; get; }
        public int Id { set; get; }
        public string C_Name { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public win_add_edit_customers()
        {
            InitializeComponent();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void txt_no_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_date.Content = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text));
            lbl_fullname.Content = PublicVar.GlobalForName + " " + PublicVar.GlobalSurName;
            txt_name.Focus(); 
            if(Type==false)
            {
                txt_address.Text = this.Address;
                txt_name.Text = this.C_Name;
                txt_no.Text = this.Phone;
                lbl_title.Content = "فرم ویرایش مشتری";
            }
        }
        private Boolean Checkable()
        {
            if(txt_name.Text.Trim()==string.Empty)
            {
                MessageBox.Show("لطفا نام مشتری را وارد نمایید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_name.Focus();
                return false;
            }
            if (txt_no.Text.Trim() == string.Empty)
            {
                MessageBox.Show("لطفا شماره مشتری را وارد نمایید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_no.Focus();
                return false;
            }
            if (txt_address.Text.Trim() == string.Empty)
            {
                MessageBox.Show("لطفا آدرس مشتری را وارد نمایید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_address.Focus();
                return false;
            }
            return true;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (!Checkable())
                return;
            else
            {
                try
                {
                    if(Type==true)
                    {
                        Customer customer = new Customer();
                        customer.Mobile = txt_no.Text.Trim();
                        customer.Name = txt_name.Text.Trim();
                        customer.StartDate = lbl_date.Content.ToString();
                        customer.UserId = PublicVar.GlobalUserId;
                        customer.Address = txt_address.Text.Trim();
                        customer.IsActive = 1;
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        MessageBox.Show("اطلاعات شما با موفقیت ثبت گردید", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    }
                   else if(Type==false)
                    {
                        Customer Query = (from u in db.Customers where u.Id==this.Id select u).SingleOrDefault();
                        Query.Mobile = txt_no.Text.Trim();
                        Query.Name = txt_name.Text.Trim();
                        Query.StartDate = lbl_date.Content.ToString();
                        Query.Address = txt_address.Text.Trim();
                        Query.UserId = PublicVar.GlobalUserId;
                        
                        db.SaveChanges();
                        MessageBox.Show("اطلاعات شما با موفقیت بروزرسانی گردید", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                finally
                {
                    this.Close();
                }
            }
        }
    }
}
