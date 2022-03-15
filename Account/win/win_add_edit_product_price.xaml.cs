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
using System.Transactions;
using DateModel;
using Account.Class;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_add_edit_product_price.xaml
    /// </summary>
    public partial class win_add_edit_product_price : Window
    {
        public int ID { set; get; }
        public string name { set; get; }
        AccountEntities db = new AccountEntities();
        public win_add_edit_product_price()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_delproduct_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_addproduct_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
            {
                try
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        PRODUCTPRICE pRODUCTPRICE = new PRODUCTPRICE();
                        pRODUCTPRICE.BUYPRICE =long.Parse( txt_buy.Text.Trim());
                        pRODUCTPRICE.DISCRIPTION = txt_dis.Text.Trim();
                        pRODUCTPRICE.SELLPRICE = long.Parse(txt_sell.Text.Trim());
                        pRODUCTPRICE.PRODUCTSID = ID;
                        pRODUCTPRICE.USERSID = PublicVar.GlobalUserId;
                        pRODUCTPRICE.STARTDATE = lbl_date.Content.ToString();
                        db.PRODUCTPRICEs.Add(pRODUCTPRICE);
                        db.SP_UPDATE_PRODUCTS(ID, long.Parse(txt_sell.Text.Trim()),Convert.ToInt64( txt_buy.Text.Trim()));
                        db.SaveChanges();
                        
                        ts.Complete();
                        MessageBox.Show("اطلاعات شما با موفقیت در ثبت گردید", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                }
                finally
                {
                    this.Close();
                }
            }
            else
                return;
        }
        private Boolean Check()
        {
            if(string.IsNullOrWhiteSpace(txt_buy.Text))
            {
                txt_buy.Focus();
                MessageBox.Show("لطفا قیمت خرید را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_sell.Text))
            {
                txt_sell.Focus();
                MessageBox.Show("لطفا قیمت فروش را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_dis.Text))
            {
                txt_dis.Focus();
                MessageBox.Show("لطفا توضییحات مربوط به کالا را وارد کنید را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                return false;
            }
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_date.Content = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text));
            lbl_fullname.Content = PublicVar.GlobalForName + " " + PublicVar.GlobalSurName;
            lbl_title.Content = name;
            txt_buy.Focus();
        }

        private void txt_buy_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_sell_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
