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
using DateModel;
using System.Windows.Shapes;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_products.xaml
    /// </summary>
    public partial class win_products : Window
    {
        AccountEntities db = new AccountEntities();
        public win_products()
        {
            InitializeComponent();
        }
        private void btn_show_product_price( object sender,RoutedEventArgs e)
        {

            var obj = datagrid_pro.SelectedItem;
            win_product_price win_Product_Price = new win_product_price();
            win_Product_Price.ProId = Convert.ToInt32((datagrid_pro.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
            win_Product_Price.ProName = (datagrid_pro.SelectedCells[1].Column.GetCellContent(obj) as TextBlock).Text;
            win_Product_Price.ShowDialog();
            ShowInfo(State);
        }
        private void btn_show_transaction(object sender, RoutedEventArgs e)
        {

            var obj = datagrid_pro.SelectedItem;
            win_transaction transaction = new win_transaction();
            transaction.ID = Convert.ToInt32((datagrid_pro.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
            transaction.Suply= Convert.ToInt32((datagrid_pro.SelectedCells[4].Column.GetCellContent(obj) as TextBlock).Text);
            transaction.Pname = (datagrid_pro.SelectedCells[1].Column.GetCellContent(obj) as TextBlock).Text;
            transaction.ShowDialog();
            ShowInfo(State);
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            ShowInfo(State);
        }
        private void ShowInfo(Func<string> input)
        {
            var query = db.Database.SqlQuery<VW_PRODUCTS>("SELECT * FROM VW_PRODUCTS WHERE "+input());
            var run = query.ToList();
            datagrid_pro.ItemsSource = run;
        }
        private string State()
        {
            string result = "ProductStartDate BETWEEN '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_from.Text)) + "' AND '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to.Text)) + "' AND ProductActive = 1 ";
            if (!string.IsNullOrWhiteSpace(txt_name.Text))
                result += "AND ProductName LIKE '%"+txt_name.Text.Trim()+"%' ";
            if (cmb_state.SelectedIndex == 1)
                result += "AND ProductLastSuply>=1";
            else if (cmb_state.SelectedIndex == 2)
                result += "AND ProductLastSuply<=0";
            return result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmb_state.Items.Add("همه");
            cmb_state.Items.Add("موجود");
            cmb_state.Items.Add("نا موجود");
            cmb_state.SelectedIndex = 0;
            ShowInfo(State);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            win_add_edit_product p = new win_add_edit_product();
            p.Type = 1;
            p.ShowDialog();
            ShowInfo(State);
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {//////// These codes are for editing products
            object obj = datagrid_pro.SelectedItem;
            if (obj != null)
            {
                try
                {
                    win_add_edit_product p = new win_add_edit_product();
                    p.ID = int.Parse((datagrid_pro.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);

                    p.ProName = (datagrid_pro.SelectedCells[1].Column.GetCellContent(obj) as TextBlock).Text;
                    p.ProDisc = (datagrid_pro.SelectedCells[2].Column.GetCellContent(obj) as TextBlock).Text;
                    p.Type = 2;
                    p.ShowDialog();
                    ShowInfo(State);
                }
                catch
                {
                    MessageBox.Show("لطفا ردیف مناسب را انتخاب نمایید");
                }
            
               
              
            }
            else
                MessageBox.Show("لطفا کالای مورد نظر را انتخاب کنید");
            
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            object obj = datagrid_pro.SelectedItem;
            if (obj != null)
            {
                if (MessageBox.Show("آیا از حذف کالای موردنظر مطمین هستید؟", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    var Lid = byte.Parse((datagrid_pro.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
                    if (Lid != null)
                    {
                        PRODUCT U = (from p in db.PRODUCTS where Lid == p.ProductId select p).SingleOrDefault();
                        U.ProductActive = 2;
                        db.SaveChanges();
                        MessageBox.Show("کالای مورد نظر با موفقیت حذف گردید");
                    }
                }
            }

        }
    }
}
