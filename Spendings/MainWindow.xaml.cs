using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Spendings
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Эко_РегионыEntities db = new Эко_РегионыEntities();

        private bool avaiblediogram = false;
        private double tekushidiogramm, investdiogramm, spenddiogeamm;
        private int iddiogram;

        public MainWindow()
        {
            InitializeComponent();

            List<regions_> allregions = db.regions_.ToList();

            for (int i = 0; i < allregions.Count; i++)
            {
                regionpicker.Items.Add(allregions[i].Название.TrimStart());
            }
        }

        private void yearpicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            //Все расходы
            List<spendings_> allspending = db.spendings_.ToList();

            show.Items.Clear();
            izabsolut.Text = string.Empty;
            izpercent.Text = string.Empty;

            string selectedyear = "";
            int selectedregion = 0;

            switch (yearpicker.SelectedIndex)
            {
                case 0:
                    selectedyear = "2019";
                    break;

                case 1:
                    selectedyear = "2020";
                    break;

                case 2:
                    selectedyear = "2021";
                    break;
            }

            selectedregion = regionpicker.SelectedIndex + 1;

            if (regionpicker.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите регион");
                regionpicker.Text = string.Empty;
            }
            else
            {
                //Нужные расходы
                List<spendings_> neededspend = allspending.Where(x => x.Год == selectedyear && x.Код_региона == selectedregion).ToList();

                int counted = 1;
                for (int i = 0; i < neededspend.Count; i++)
                {
                    if (counted == 1)
                    {
                        //Пропись
                        int neededcode = neededspend[i].regions_.Код;
                        string neededname = neededspend[i].regions_.Название;
                        double neededtek = neededspend[i].Сумма.Value;
                        double neededinvest = neededspend[i + 1].Сумма.Value;
                        double neededspendkap = neededspend[i + 2].Сумма.Value;
                        double neededsumma = neededtek + neededinvest + neededspendkap;

                       show.Items.Add(new Data { Code = neededcode, 
                           Name = neededname, Tekush = neededtek, Invest = neededinvest, SpendKap = neededspendkap, Summa = neededsumma});

                        iddiogram = neededspend[i].Код;
                        tekushidiogramm = neededspend[i].Сумма.Value;
                        investdiogramm = neededspend[i + 1].Сумма.Value;
                        spenddiogeamm = neededspend[i + 2].Сумма.Value;

                        avaiblediogram = true;

                        counted += 1;
                    }
                }

                List<spendings_> spendforcounting = allspending.Where(x => x.Код_региона == selectedregion).ToList();

                if (selectedyear == "2019")
                {
                    izlabel.Text = "Изменение суммарных природоохранных расходов в 2019 г. к 2018 г., %:";
                    izabsltlabel.Text = "Изменение суммарных природоохранных расходов в 2019 г. к 2018 г., абсолют. В рублях:";

                    izabsolut.Text = "Нет данных";
                    izabsltlabel.Text = "Нет данных";
                }
                else if (selectedyear == "2020")
                {
                    double sum2020 = spendforcounting[5].Сумма.Value + spendforcounting[4].Сумма.Value + spendforcounting[3].Сумма.Value;
                    double sum2019 = spendforcounting[2].Сумма.Value + spendforcounting[1].Сумма.Value + spendforcounting[0].Сумма.Value;

                    izlabel.Text = "Изменение суммарных природоохранных расходов в 2020 г. к 2019 г., %:";
                    izabsltlabel.Text = "Изменение суммарных природоохранных расходов в 2020 г. к 2019 г., абсолют. В рублях:";

                    izabsolut.Text = Convert.ToString(sum2020 - sum2019);
                    izpercent.Text = Convert.ToString(Math.Round((sum2020 / sum2019 - 1) * 100, 2));
                }
                else
                {
                    double sum2021 = spendforcounting[8].Сумма.Value + spendforcounting[7].Сумма.Value + spendforcounting[6].Сумма.Value;
                    double sum2020 = spendforcounting[5].Сумма.Value + spendforcounting[4].Сумма.Value + spendforcounting[3].Сумма.Value;

                    izlabel.Text = "Изменение суммарных природоохранных расходов в 2021 г. к 2020 г., %:";
                    izabsltlabel.Text = "Изменение суммарных природоохранных расходов в 2021 г. к 2020 г., абсолют. В рублях:";

                    izabsolut.Text = Convert.ToString(sum2021 - sum2020);
                    izpercent.Text = Convert.ToString(Math.Round((sum2021 / sum2020 - 1) * 100, 2));
                }

            }
        }

        public class Data
        {
            public int Code { get; set; }
            public string Name { get; set; }
            public double Tekush { get; set; }
            public double Invest { get; set; }
            public double SpendKap { get; set; }
            public double Summa { get; set; } 
        }

        private void showallreg_Click(object sender, RoutedEventArgs e)
        {
            show.Items.Clear();

            avaiblediogram = false;
            //Все расходы
            List<spendings_> allspending = db.spendings_.ToList();

            int counted = 1, dop = 0, alsodop = 1;

            for (int i = 0; i < allspending.Count; i++)
            {
                if (counted == 1)
                {
                    //Пропись
                    show.Items.Add(new Data { Code = alsodop, 
                        Name = allspending[dop].regions_.Название.TrimStart(), 
                        Tekush = allspending[dop].Сумма.Value, 
                        Invest = allspending[dop + 1].Сумма.Value, 
                        SpendKap = allspending[dop + 2].Сумма.Value, 
                        Summa = allspending[dop].Сумма.Value + allspending[dop + 1].Сумма.Value + allspending[dop + 2].Сумма.Value });

                    dop += 9;

                    alsodop++;

                    counted++;
                }
                else
                {
                
                    if (counted % 10 == 0)
                    {
                        counted = 1;
                    }
                    else
                    {
                        counted++;
                    }
                }
            }
        }

        private void showdiagram_Click(object sender, RoutedEventArgs e)
        {
            if (!avaiblediogram)
            {
                MessageBox.Show("Выберите нужный регион и год");
            }
            else
            {
                Diagram diagram = new Diagram(iddiogram, tekushidiogramm, investdiogramm, spenddiogeamm);
                diagram.Show();
            }
        }
    }
}
