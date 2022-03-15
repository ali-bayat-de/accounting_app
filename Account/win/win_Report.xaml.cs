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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_Report.xaml
    /// </summary>
    public partial class win_Report : Window
    {
        public string ReportPath { set; get; }
        public string FormulaString { set; get; }
        public string[] Parameter = new string[5];
        public win_Report()
        {
            InitializeComponent();
        }
        private void SetSize()
        {
            this.MaxHeight = 700;
            this.MinHeight = 700;
            this.MaxWidth = 1200;
            this.MaxWidth = 1200;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rpt.Owner = Window.GetWindow(this);
            SetSize();
            ReportDocument reportDocument = new ReportDocument();
            string Path = System.AppDomain.CurrentDomain.BaseDirectory + "Reports\\" + this.ReportPath;
            reportDocument.Load(Path);
            reportDocument.RecordSelectionFormula = FormulaString;
            switch (ReportPath)
            {
                case "Custoemr.rpt":
                    {
                       
                        reportDocument.SetParameterValue("ReportDate", string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)));
                    }
                    break;
                case "Product.rpt":
                    {
                        reportDocument.SetParameterValue("From_Date", Parameter[4]);
                        reportDocument.SetParameterValue("To_Date", Parameter[5]);
                        reportDocument.SetParameterValue("ReportDate", string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)));
                    }
                    break;
                case "User.rpt":
                    {
                        reportDocument.SetParameterValue("From_Date", Parameter[2]);
                        reportDocument.SetParameterValue("To_Date", Parameter[3]);
                        reportDocument.SetParameterValue("ReportDate", string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)));
                    }
                    break;
                case "Sale_All.rpt":
                    {
                        reportDocument.SetParameterValue("date_from",  Parameter[0]);
                        reportDocument.SetParameterValue("date_to", Parameter[1]);
                        reportDocument.SetParameterValue("create_report_date", string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)));
                    }

                    break;
                case "Sale_All_Customer.rpt":
                    {
                        reportDocument.SetParameterValue("date_from", Parameter[0]);
                        reportDocument.SetParameterValue("date_to", Parameter[1]);
                        reportDocument.SetParameterValue("create_report_date", string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)));
                    }
                    break;
            }
           
            rpt.ViewerCore.ReportSource = reportDocument;
        }
    }
}
