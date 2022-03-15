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
    /// Interaction logic for win_UsersLog.xaml
    /// </summary>
    public partial class win_UsersLog : Window
    {
        AccountEntities db = new AccountEntities();
        public win_UsersLog()
        {
            InitializeComponent();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ShowInfo(Func<string> input)
        {
            var query = db.Database.SqlQuery<VW_USERSLOG>("SELECT * FROM VW_USERSLOG WHERE "+input());
            var run = query.ToList();
            dg_userslog.ItemsSource = run;
        }
        private string Input()
        {
            string Time = cmb_hour_f.SelectedValue + ":" + cmb_minite_f2.SelectedValue + ":" + cmb_second_f.SelectedValue;
            string Time2 = cmb_hour_s.SelectedValue + ":" + cmb_minete_s.SelectedValue + ":" + cmb_second_s.SelectedValue;
            string Result = "STARTDATE BETWEEN '"+string.Format("{0:yyyy/MM/dd}",Convert.ToDateTime(cal_from.Text))+" - "+Time+ "' AND '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to.Text)) + " - " + Time2 + "' ";
            if (txt_ip.Text.Trim() != string.Empty)
                Result += "AND IPADDRESS LIKE '%"+txt_ip.Text.Trim()+"%' ";
            if (cmb_username.Text != string.Empty)
                Result += "AND USERSID = "+cmb_username.SelectedValue;
            return Result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Set_Time();
            Show_Members();
            ShowInfo(Input);

        }
        private void Show_Members()
        {
            cmb_username.ItemsSource = db.VW_USERS.ToList();
            cmb_username.DisplayMemberPath = "FULLNAME";
            cmb_username.SelectedValuePath = "USERID";
            cmb_username.SelectedIndex = 0;
        }
        private void Set_Time()
        {
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    cmb_hour_f.Items.Add("0" + i);
                    cmb_hour_s.Items.Add("0" + i);
                }

                else
                {
                    cmb_hour_f.Items.Add(i);
                    cmb_hour_s.Items.Add(i);
                }

            }
            for (int i=0;i<60;i++)
            {
                if (i < 10)
                {
                    cmb_minite_f2.Items.Add("0" + i);
                    cmb_minete_s.Items.Add("0" + i);
                    /////////////////second
                    cmb_second_f.Items.Add("0" + i);
                    cmb_second_s.Items.Add("0" + i);
                }

                else
                {
                    cmb_minite_f2.Items.Add(i);
                    cmb_minete_s.Items.Add(i);
                    /////////////////second
                    cmb_second_f.Items.Add(i);
                    cmb_second_s.Items.Add(i);
                }
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo(Input);
        }
    }
}
