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
    /// Interaction logic for win_transaction.xaml
    /// </summary>
    public partial class win_transaction : Window
    {
        AccountEntities db = new AccountEntities();
        public int ID { set; get; }
        public string Pname { set; get; }
        public int Suply { set; get; }
        public win_transaction()
        {
            InitializeComponent();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ShowInfo(Func<string> input)
        {
            var query = db.Database.SqlQuery<VW_TRANSACTION>("SELECT * FROM VW_TRANSACTION WHERE PRODUCTSID = "+ID+" AND "+input());
            var run = query.ToList();
            dg_trans.ItemsSource = run;
        }
        private string Input()
        {
            string Result = "STARTDATE BETWEEN '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_from.Text)) + "' AND '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to.Text)) + "' ";
            if (cmb_state.SelectedIndex == 1)
                Result += "AND COUNT>=1 ";
            else if (cmb_state.SelectedIndex == 2)
                Result += "AND COUNT<0 ";
            if (!string.IsNullOrWhiteSpace(txt_fullname.Text))
                Result += "AND FULLNAME LIKE '%"+txt_fullname.Text.Trim()+"%' ";
            return Result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_proname.Content = Pname;
            cmb_state.Items.Add("همه");
            cmb_state.Items.Add("افزایش موجودی");
            cmb_state.Items.Add("کاهش موجودی");
            cmb_state.SelectedIndex = 0;
            ShowInfo(Input);
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            ShowInfo(Input);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            win_transaction_add  win = new win_transaction_add();
            win.ID = this.ID;
            win.Pname = this.Pname;
            win.Suply = this.Suply;
            win.ShowDialog();
            ShowInfo(Input);
        }
    }
}
