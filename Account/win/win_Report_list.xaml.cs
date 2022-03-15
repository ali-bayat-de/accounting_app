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
    /// Interaction logic for win_Report_list.xaml
    /// </summary>
    public partial class win_Report_list : Window
    {
        AccountEntities db = new AccountEntities();
        win_Report report;
        public string GetReportName { set; get; }
        public win_Report_list()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void img_exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void lst_report_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_select.IsEnabled = true;
            if(lst_report.SelectedIndex==0)
            {
                btn_select.IsEnabled = false;
                CheckVisibility();
            }
           
           else if (lst_report.SelectedIndex == 3)
            {
                CheckVisibility();
                GetReportName = "Custoemr.rpt";
                grb_customers.Visibility = Visibility.Visible;
            }
            else if (lst_report.SelectedIndex == 4)
            {
                CheckVisibility();
                GetReportName = "Product.rpt";
                grb_products_report.Visibility = Visibility.Visible;
            }
            else if (lst_report.SelectedIndex == 5)
            {
                CheckVisibility();
                GetReportName = "User.rpt";
                grb_users_report.Visibility = Visibility.Visible;
            }
            else if (lst_report.SelectedIndex == 6)
            {
                btn_select.IsEnabled = false;
                CheckVisibility();
            }
            else if(lst_report.SelectedIndex==9)
            {
                CheckVisibility();
                grb_sell_report.Visibility = Visibility.Visible;
                GetReportName = "Sale_All.rpt";
                cmb_select_customer.ItemsSource = db.Customers.ToList();
                cmb_select_customer.DisplayMemberPath = "Name";
                cmb_select_customer.SelectedValuePath = "Id";
                cmb_select_customer.SelectedIndex = 0;
            }
            else if (lst_report.SelectedIndex == 10)
            {
                CheckVisibility();
                grb_sell_report.Visibility = Visibility.Visible;
                GetReportName = "Sale_All_Customer.rpt";
                cmb_select_customer.ItemsSource = db.Customers.ToList();
                cmb_select_customer.DisplayMemberPath = "Name";
                cmb_select_customer.SelectedValuePath = "Id";
                cmb_select_customer.SelectedIndex = 0;
            }

        }
        private void show_index10(object sender,RoutedEventArgs e)
        {
            CheckVisibility();
            grb_sell_report.Visibility = Visibility.Visible;
            GetReportName = "Sale_All_Customer.rpt";
            cmb_select_customer.ItemsSource = db.Customers.ToList();
            cmb_select_customer.DisplayMemberPath = "Name";
            cmb_select_customer.SelectedValuePath = "Id";
            cmb_select_customer.SelectedIndex = 0;
        }
        private void show_index9(object sender,RoutedEventArgs e)
        {
            CheckVisibility();
            grb_sell_report.Visibility = Visibility.Visible;
            GetReportName = "Sale_All.rpt";
            cmb_select_customer.ItemsSource = db.Customers.ToList();
            cmb_select_customer.DisplayMemberPath = "Name";
            cmb_select_customer.SelectedValuePath = "Id";
            cmb_select_customer.SelectedIndex = 0;
        }
        
        private void show_index6(object sender, RoutedEventArgs e)
        {
            btn_select.IsEnabled = false;
            CheckVisibility();
        }
        private void show_index5(object sender,RoutedEventArgs e)
        {
            CheckVisibility();
            GetReportName = "User.rpt";
            grb_users_report.Visibility = Visibility.Visible;
        }
         private void show_index4(object sender,RoutedEventArgs e)
        {
            CheckVisibility();
            GetReportName = "Product.rpt";
            grb_products_report.Visibility = Visibility.Visible;
        }
        private void show_index3(object sender, RoutedEventArgs e)
        {
            CheckVisibility();
            GetReportName = "Custoemr.rpt";
            grb_customers.Visibility = Visibility.Visible;
        }
        private void CheckVisibility()
        {
            grb_sell_report.Visibility = Visibility.Hidden;
            grb_customers.Visibility = Visibility.Hidden;
            grb_products_report.Visibility = Visibility.Hidden;
            grb_users_report.Visibility = Visibility.Hidden;
        }
        private void btn_select_Click(object sender, RoutedEventArgs e)
        {
            report = new win_Report();
            report.ReportPath = GetReportName;
            report.FormulaString = FormulaCheck();
            report.ShowDialog();
        }
        private string FormulaCheck()
        {
            string Formula = string.Empty;
            switch(GetReportName)
            {
                case "Custoemr.rpt":
                    {
                        
                        if (rdb_active.IsChecked == true)
                        {
                            Formula = " and {VW_CUSTOMERS.IsActive} = 1";
                            
                        }
                        else if(rdb_deactive.IsChecked==true)
                            Formula = " and {VW_CUSTOMERS.IsActive} = 2";
                     }
                    break;
                case "Product.rpt":
                    {
                        report.Parameter[4] = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_from.Text));
                        report.Parameter[5] = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to.Text));
                        Formula = "{VW_USERS.USERSTARTDATE} in '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_from.Text)) + "' to '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to.Text)) + "'";
                        if (rdb_exist.IsChecked == true)
                        {
                            Formula += " and {VW_PRODUCTS.ProductLastSuply} >= 1";

                        }
                        else if (rdb_notexist.IsChecked == true)
                            Formula += " and {VW_PRODUCTS.ProductLastSuply} <= 0";
                    }
                    break;
                case "User.rpt":
                    {
                        report.Parameter[2] = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_from2.Text));
                        report.Parameter[3] = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to2.Text));
                        Formula = "{VW_USERS.USERSTARTDATE} in '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_from2.Text)) + "' to '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to2.Text)) + "'";
                        if (rdb_deactive_user.IsChecked == true)
                        {
                            Formula += " and {VW_USERS.USERACTIVE} = 2";

                        }
                        else if (rdb_active_user.IsChecked == true)
                            Formula += " and {VW_USERS.USERACTIVE} = 1";
                        
                    }
                    break;
                case "Sale_All.rpt":
                    {
                        
                        report.Parameter[0] = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_az.Text));
                        report.Parameter[1] = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_ta.Text));
                        Formula = "{VW_FACTORS.StartDate} in '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_az.Text)) + "' to '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_ta.Text)) + "' and";
                        if (rdb_back_factor.IsChecked == true)
                            Formula += " not {VW_FACTORS.PurchType} and";
                        else if (rdb_sell_factor.IsChecked == true)
                            Formula += " {VW_FACTORS.PurchType} and";
                        Formula += " {VW_FACTORS.CustomerId} = " + cmb_select_customer.SelectedValue;
                       // MessageBox.Show(cmb_select_customer.SelectedValue.ToString());
                    }
                    break;
                case "Sale_All_Customer.rpt":
                    {

                        report.Parameter[0] = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_az.Text));
                        report.Parameter[1] = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_ta.Text));
                        Formula = "{VW_FACTORS.StartDate} in '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_az.Text)) + "' to '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_ta.Text)) + "' and";
                        if (rdb_back_factor.IsChecked == true)
                            Formula += " not {VW_FACTORS.PurchType} and";
                        else if (rdb_sell_factor.IsChecked == true)
                            Formula += " {VW_FACTORS.PurchType} and";
                        Formula += " {VW_FACTORS.CustomerId} = " + cmb_select_customer.SelectedValue;
                        // MessageBox.Show(cmb_select_customer.SelectedValue.ToString());
                    }
                    break;
            }
            return Formula;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
