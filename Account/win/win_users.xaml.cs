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
    /// Interaction logic for win_users.xaml
    /// </summary>
    public partial class win_users : Window
    {
        AccountEntities db = new AccountEntities();
        public win_users()
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
        private void ShowInfo(Func<string> input)
        {
            var query = db.Database.SqlQuery<VW_USERS>("SELECT * FROM VW_USERS WHERE "+input());
            var run = query.ToList();
            datagrid_users.ItemsSource = run;
        }
        private string Input()
        {
            string result = "USERSTARTDATE BETWEEN '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_from.Text)) + "' AND '" + string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal_to.Text)) + "' ";
            if (!string.IsNullOrWhiteSpace(txt_name.Text))
                result += "AND USERNAME LIKE '%"+txt_name.Text.Trim()+"%' ";
            if (!string.IsNullOrWhiteSpace(txt_lastname.Text.Trim()))
                result += "AND USERFAMILY LIKE '%"+txt_lastname.Text.Trim()+"%' ";
            if (cmb_state.SelectedIndex == 1)
                result += "AND USERACTIVE = 1 ";
            else if (cmb_state.SelectedIndex == 2)
                result += "AND USERACTIVE = 2";
            return result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmb_state.Items.Add("همه");
            cmb_state.Items.Add("فعال");
            cmb_state.Items.Add("غیرفعال");
            cmb_state.SelectedIndex = 0;
            ShowInfo(Input);
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            ShowInfo(Input);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            win_add_edit_users u = new win_add_edit_users();
            u.Type = 1;
            u.ShowDialog();
            ShowInfo(Input);
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            object obj = datagrid_users.SelectedItem;
            if (obj != null)
            {
                try
                {
                    win_add_edit_users u = new win_add_edit_users();
                    u.ID = Convert.ToInt32(((datagrid_users.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text));
                    u.Type = 2;
                    u.ShowDialog();
                }
                catch
                {
                    MessageBox.Show("لطفا یک ردیف صحیح را انتخاب نمایید");
                }
               
            }
            else
                MessageBox.Show("لطفا یک ردیف را انتخاب نمایید");
            
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            
                object obj = datagrid_users.SelectedItem;
                if (obj != null)
                {
                    if (MessageBox.Show("آیا از حذف کاربر موردنظر مطمین هستید؟", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                        {
                    byte Lid = byte.Parse((datagrid_users.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
                        if (Lid != null)
                            {
                        USER U = (from p in db.USERS where Lid == p.USERID select p).SingleOrDefault();
                        U.USERACTIVE = 2;
                        db.SaveChanges();
                        MessageBox.Show("کاربر مورد نظر با موفقیت حذف گردید");
                            }
                        }
                }
            
            
        }
    }
}
