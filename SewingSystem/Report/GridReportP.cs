using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors;
using System.Linq;
using SewingSystem.Classes;
using System.Windows.Forms;
using static SewingSystem.Classes.Session;

namespace SewingSystem.Reports
{
    public partial class GridReportP : XtraReport 
    {
        public GridReportP()
        {
            InitializeComponent();
            objectDataSource2.DataSource = Program.Branch;
        }

        public static void Print(GridControl gridControl1, string ReportName, string filter)
        {

            GridView view = gridControl1.MainView as GridView;

            Print((IBasePrintable)gridControl1, ReportName, filter);


        }
        public static void Print(IBasePrintable printableComponent, string ReportName, string filter, bool ShowInDialog = false, bool landScape = false, PrintFileType printFile = PrintFileType.Printer)
        {
            if (printableComponent is GridControl gc && gc.MainView is GridView g)
            {
                g.OptionsView.ShowViewCaption = false;
                g.OptionsView.RowAutoHeight = true;
            }
            PrintableComponentLink pcLink1 = new PrintableComponentLink();

            pcLink1.Component = printableComponent;
            GridReportP rpt = new GridReportP();
            //rpt.LoadTemplete();
            rpt.printableComponentContainer1.PrintableComponent = pcLink1;

            rpt.Landscape = landScape;
            if (landScape)
                rpt.xrSubreport1.ReportSource = new ReportHeaderWidth("");
            else rpt.xrSubreport1.ReportSource = new ReportHeader("");
            rpt.Cell_ReportName.Text = ReportName;
            rpt.Cell_Filters.Text = filter;

            try
            {
                switch (printFile)
                {
                    case PrintFileType.Printer:
                        if (ShowInDialog)
                            rpt.ShowPreviewDialog();
                        else
                            rpt.ShowPreview();
                        break;
                    case PrintFileType.PDF:
                        SaveFileDialog.Filter = "PDF Files|*.pdf";
                        if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                            rpt.ExportToPdf(SaveFileDialog.FileName);
                        break;
                    case PrintFileType.Xlsx:
                        SaveFileDialog.Filter = "Excel Files|*.Xls";
                        if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                            rpt.ExportToXls(SaveFileDialog.FileName);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }


        }
        public static XtraSaveFileDialog SaveFileDialog = new XtraSaveFileDialog();
     
    }
}
