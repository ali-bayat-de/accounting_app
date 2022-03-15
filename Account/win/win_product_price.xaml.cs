using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using DateModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_product_price.xaml
    /// </summary>
    public partial class win_product_price : Window
    {
        AccountEntities db = new AccountEntities();
        public string ProName { set; get; }
        public int ProId { set; get; }
        public win_product_price()
        {
            InitializeComponent();
        }

        private void btn_delproduct_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_price.Content = ProName;
            ShowInfo(input);
        }
        private void ShowInfo(Func<string> func)
        {
            var query = db.Database.SqlQuery<VW_PRODUCTPRICE>("SELECT * FROM VW_PRODUCTPRICE WHERE PRODUCTSID = "+ProId+" AND "+func());
            var run = query.ToList();
            dg_pprice.ItemsSource = run;
        }
        private string input()
        {
            string result = "STARTDATE BETWEEN '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_from.Text)) + "' AND '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to.Text)) + "' "; ;
            if (!string.IsNullOrWhiteSpace(txt_fullname.Text))
                result += " AND FULLNAME LIKE '%"+txt_fullname.Text.Trim()+"%' ";
            return result;
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            ShowInfo(input);
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_addproduct_Click(object sender, RoutedEventArgs e)
        {
           
                win_add_edit_product_price ww = new win_add_edit_product_price();
                ww.ID = ProId;
                ww.name = ProName;
                ww.ShowDialog();
                ShowInfo(input);
           
        }
    }
}
