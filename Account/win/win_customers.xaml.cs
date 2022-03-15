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

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_customers.xaml
    /// </summary>
    public partial class win_customers : Window
    {
        AccountEntities db = new AccountEntities();
        public win_customers()
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
        private void ShowInfo(Func<String> input)
        {
            var Query = db.Database.SqlQuery<VW_CUSTOMERS>("SELECT * FROM VW_CUSTOMERS WHERE IsActive=1 "+input());
            var Run = Query.ToList();
            dg_customer.ItemsSource = Run;
        }
        private string Input()
        {
            string Result =string.Empty;
            if (txt_name.Text.Trim() != string.Empty)
                Result += "AND Name LIKE '%"+txt_name.Text.Trim()+"%' ";
            if (txt_address.Text.Trim() != string.Empty)
                Result += "AND Address LIKE '%"+txt_address.Text.Trim()+"%' ";
            return Result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowInfo(Input);
        }

        private void img_search_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowInfo(Input);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            win_add_edit_customers win = new win_add_edit_customers();
            win.Type = true;
            win.ShowDialog();
            ShowInfo(Input);
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var Obj = dg_customer.SelectedItem;
            if (Obj != null)
            {
                win_add_edit_customers win = new win_add_edit_customers();
                win.Type = false;
                win.Id = int.Parse((dg_customer.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text);
                win.C_Name = (dg_customer.SelectedCells[1].Column.GetCellContent(Obj) as TextBlock).Text;
                win.Address = (dg_customer.SelectedCells[3].Column.GetCellContent(Obj) as TextBlock).Text;
                win.Phone = (dg_customer.SelectedCells[2].Column.GetCellContent(Obj) as TextBlock).Text;
                win.ShowDialog();
                ShowInfo(Input);
            }
            else
                MessageBox.Show("لطفا مشتری موردنظر جهت ویرایش را انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            object Obj = dg_customer.SelectedItem;
            if(Obj!=null)
            {
                if(MessageBox.Show("آیا از حذف مشتری موردنظر اطمینان دارید؟", "سوال", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign)==MessageBoxResult.Yes)
                {
                    int Id = int.Parse((dg_customer.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text);
                    Customer customer = (from c in db.Customers where c.Id==Id select c).SingleOrDefault();
                    customer.IsActive = 2;
                    db.SaveChanges();
                    MessageBox.Show("مشتری موردنظر با موفقیت حذف گردید.", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    ShowInfo(Input);
                }
            }
            else
                MessageBox.Show("لطفا مشتری موردنظر جهت حذف را انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
        }
    }
}
