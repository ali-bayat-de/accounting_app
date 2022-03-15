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
using Account.Class;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DateModel;
using Account.win;

namespace Account
{
    /// <summary>
    /// Interaction logic for win_main.xaml
    /// </summary>
    public partial class win_main : Window
    {
        AccountEntities db = new AccountEntities();
        public win_main()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Set_EndTime();
            Environment.Exit(0);
        }
        private void btn_exit(object sender,RoutedEventArgs e)
        {
            Set_EndTime();
            Environment.Exit(0);
        }
        private void btn_show_factor_list(object sender, RoutedEventArgs e)
        {
            win_Factors factors = new win_Factors();
            factors.ShowDialog();
        }
        private void btn_show_report_list(object sender, RoutedEventArgs e)
        {
            win_Report_list list = new win_Report_list();
            list.ShowDialog();
        }
        //
        private void btn_show_factor(object sender,RoutedEventArgs e)
        {
            win_Factor win_Factor = new win_Factor();
            win_Factor.Type = true;
            win_Factor.ShowDialog();
        }
        private void btn_show_customers(object sender, RoutedEventArgs e)
        {
            win_customers customers = new win_customers();
            customers.ShowDialog();
        }
        private void btn_change_pass(object sender,RoutedEventArgs e)
        {
            win_change_password password = new win_change_password();
            password.ShowDialog();
        }
        private void btn_show_userslog(object sender, RoutedEventArgs e)
        {
            win_UsersLog ul = new win_UsersLog();
            ul.ShowDialog();
        }
        private void Set_EndTime()
        {
            db.SP_UPDATE_USERLOG(PublicVar.GlobalUserId, string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)) + " - " + string.Format("{0:HH:mm:ss}", DateTime.Now));
            db.SaveChanges();
        }
        private void btn_show_users(object sender, RoutedEventArgs e)
        {
            win_users u = new win_users();
            u.ShowDialog();
        }
        private void btn_show_products(object sender, RoutedEventArgs e)
        {
            win_products p = new win_products();
            p.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_family.Content = PublicVar.GlobalSurName;
            lbl_name.Content = PublicVar.GlobalForName;
            lbl_time.Content = string.Format("{0:HH:mm}",Convert.ToDateTime(DateTime.Now));
            SetSize();
        }
        private void SetSize()
        {
            this.MaxHeight = 700;
            this.MinHeight = 700;
            this.MaxWidth = 1200;
            this.MinWidth = 1200;
        }
    }
}
