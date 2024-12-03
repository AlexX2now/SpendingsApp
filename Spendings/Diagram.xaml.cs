using LiveCharts;
using LiveCharts.Wpf;
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
using LiveCharts.Wpf.Charts.Base;
using static Spendings.MainWindow;


namespace Spendings
{
    /// <summary>
    /// Логика взаимодействия для Diagram.xaml
    /// </summary>
    public partial class Diagram : Window
    {
        Эко_РегионыEntities db = new Эко_РегионыEntities();
        
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }


        public Diagram(int id, double tekush, double invest, double spent)
        {
            InitializeComponent();

            List<spendings_> allspendings = db.spendings_.ToList();

            List<spendings_> needed = allspendings.Where(x => x.Код == id).ToList();

            nameofregion.Text = needed[0].regions_.Название + " " + needed[0].Год + "г.";


            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "",
                    Values = new ChartValues<double>{tekush, invest, spent}
                }
            };

            Formatter = value => value.ToString("N");

            DataContext = this;
        }
    }
}
