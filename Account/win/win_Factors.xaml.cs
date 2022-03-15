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
using DateModel;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Account.Class;
using System.Text.RegularExpressions;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_Factors.xaml
    /// </summary>
    public partial class win_Factors : Window
    {
        AccountEntities db = new AccountEntities();
        public win_Factors()
        {
            InitializeComponent();
        }

        private void txt_phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void ShowData(Func<string> Input)
        {
            var Query = db.Database.SqlQuery<VW_FACTORS>("SELECT * FROM VW_FACTORS WHERE UserId = "+PublicVar.GlobalUserId+" AND "+Input());
            var Result = Query.ToList();
            dg_factor.ItemsSource = Result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowData(SearchString);
            
        }
        private string SearchString()
        {
            string Result = "StartDate BETWEEN '"+string.Format("{0:yyyy/MM/dd}",Convert.ToDateTime(cal_from.Text))+ "' AND '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to.Text)) + "' ";
            if (rdb_sell.IsChecked == true)
                Result += "AND PurchType = 1 ";
            else if (rdb_back.IsChecked == true)
                Result += "AND PurchType = 0 ";
            if (txt_name.Text != string.Empty)
                Result += "AND Name LIKE '%"+txt_name.Text.Trim()+"%' ";
            if (txt_phone.Text != string.Empty)
                Result += "AND Mobile LIKE '%"+txt_phone.Text.Trim()+"%' ";
            return Result;
        }

        private void btn_back1_Click(object sender, RoutedEventArgs e)
        {
            var Obj = dg_factor.SelectedItem;
            if (((dg_factor.SelectedCells[4].Column.GetCellContent(Obj) as TextBlock).Text == "بازگشتی"))
            {
                MessageBox.Show("فاکتور موردنظر بازگشتی است");
                return;
            }
            if (((dg_factor.SelectedCells[4].Column.GetCellContent(Obj) as TextBlock).Text == string.Empty))
            {
                MessageBox.Show("لطفا یک فاکتور معتبر را انتخاب نمایید");
                return;
            }
            if (MessageBox.Show("آیا از انجام این عملیات اطمینان دارید؟","هشدار",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No,MessageBoxOptions.RtlReading)==MessageBoxResult.Yes)
            {
                if (!Obj.Equals(null))
                {
                    
                    int ID = int.Parse((dg_factor.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text);
                    FACTOR Factor = (from f in db.FACTORs where f.Id==ID select f).SingleOrDefault();
                    Factor.PurchType = false;
                    db.SaveChanges();
                    MessageBox.Show("فاکتور موردنظر با موفقیت مرجوع داده شد.", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                    ShowData(SearchString);
                }
                else
                    MessageBox.Show("لطفا فاکتور موردنظر را انتخاب کنید.", "هشدار", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
            }
        }

        private void img_search_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowData(SearchString);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            win_Factor f = new win_Factor();
            f.Type = true;
            f.ShowDialog();
            ShowData(SearchString);
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            object Obj = dg_factor.SelectedItem;
            if (Obj!=null)
            {
                win_Factor factor = new win_Factor();
                factor.Type = false;
                factor.FactorId = Convert.ToInt32((dg_factor.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text);
                factor.CustomerName = (dg_factor.SelectedCells[1].Column.GetCellContent(Obj) as TextBlock).Text;
                factor.Date = (dg_factor.SelectedCells[5].Column.GetCellContent(Obj) as TextBlock).Text;
                factor.SellPrice = Convert.ToInt32((dg_factor.SelectedCells[6].Column.GetCellContent(Obj) as TextBlock).Text);
                factor.PurchPrice = Convert.ToInt32((dg_factor.SelectedCells[7].Column.GetCellContent(Obj) as TextBlock).Text);
                factor.ShowDialog();
                ShowData(SearchString);
            }
            else
                MessageBox.Show("لطفا فاکتور موردنظر را انتخاب کنید");
        }
    }
}
