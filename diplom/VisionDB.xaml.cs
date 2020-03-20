using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;
using Button = System.Windows.Controls.Button;
using DataGrid = System.Windows.Controls.DataGrid;

namespace Forms
{
    /// <summary>
    /// Логика взаимодействия для VisionDB.xaml
    /// </summary>
    /// 

    public class MyClass 
    {
        string First;
        string Second;

        public MyClass(string a, string b) {
            First = a;
            Second = b;
        }
    }
    public partial class VisionDB : Window
    {
        TimeSeriesContext context;
        private ViewDB db;
        private DBService dbService;
        public VisionDB(TimeSeriesContext context, int id)
        {
            InitializeComponent();
            this.context = context;
            dbService = new DBService(context);
            db = dbService.GetElement(id);
            LabelName.Content += db.Name;
            LabelServer.Content += db.Server;
            LabelPort.Content += db.Port;
            if (db.isMonitored == true)
            {
                InitDBIsMonitoring();
            }
            else
            {
                InitDBIsNotMonitoring();
            }
            List<StructView> collection = new List<StructView>();
            foreach (var ent in db.Entities) {
                string attr = "";
                foreach (var at in ent.Attributes) {
                    attr += $"{at.Name} \t\t - \t {at.Type}\n";
                }
                collection.Add(new StructView
                {
                    entity = ent.Name,
                    attributes = attr
                });
            }
            DataGridStruct.ItemsSource = collection;
            DataGridStruct.Visibility = Visibility.Hidden;
            ButtonStruct.Content = "Показать ▼";
            RefreshListEntities();
            chart.Width = 753;
            chart.Height = 229;
            if (db.Entities.Count > 0) {
                RefreshChart(db.Entities[0].TimeSeries);
            }
        }

        void InitDBIsMonitoring() {
            LabelLastCheck.Content = $"Последняя проверка: {db.LastCheck.ToShortDateString()} {db.LastCheck.ToShortTimeString()}";
            LabelStatus.Content = "Статус: Извлечение продолжается";
            ButtonRunCreatingTSP.Content = "Остановить извлечение временных рядов";
        }

        void InitDBIsNotMonitoring()
        {
            LabelLastCheck.Content = $"Последняя проверка: {db.LastCheck.ToShortDateString()} {db.LastCheck.ToShortTimeString()}";
            LabelStatus.Content = "Статус: Не активна";
            ButtonRunCreatingTSP.Content = "Возобновить извлечение временных рядов";
        }

        void RefreshListEntities() {
            List<Button> list = new List<Button>();
            foreach (var ent in db.Entities)
            {
                Button element = new Button();
                element.Content = ent.Name;
                element.Width = ListBoxTSEntities.Width - 8;
                element.Height = 30;
                element.Background = new SolidColorBrush(Colors.White);
                element.Click += (object sender, RoutedEventArgs e) => {
                    RefreshChart(ent.TimeSeries);
                    Background = new SolidColorBrush(Colors.MediumPurple);
                };
                list.Add(element);
            }
            ListBoxTSEntities.ItemsSource = list;
        }

        private void RefreshChart(List<ViewTimeSeriesPointEntity> tspeList) {
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(new ChartArea("TimeSeries"));
            chart.Series.Clear();
            chart.Series.Add(new Series("Count"));
            chart.Series["Count"].ChartArea = "TimeSeries";
            chart.Series["Count"].ChartType = SeriesChartType.Line;
            DateTime[] axisXData = new DateTime[tspeList.Count];
            int[] axisYData = new int[tspeList.Count];
            for (int i = 0; i < tspeList.Count; i++)
            {
                axisXData[i] = tspeList[i].Time;
                axisYData[i] = tspeList[i].Value;
            }
            chart.Series["Count"].Points.DataBindXY(axisXData, axisYData);
        }


        private void ButtonRunCreatingTSP_Click(object sender, RoutedEventArgs e)
        {
            if (db.isMonitored) {
                if (System.Windows.Forms.MessageBox.Show("Вы уверены, что хотите остановить извлечение временных рядов из этой базы данных?", "Вы уверены?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    dbService.StopCreatingTimeSeriesPoints();
                    db.isMonitored = false;
                    InitDBIsNotMonitoring();
                    System.Windows.MessageBox.Show($"Извлечение временных рядов из базы данных {db.Name} остановлено");
                }
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show("Вы уверены, что хотите возобновить извлечение временных рядов из этой базы данных? Извлечение временных рядов из текущей базы данных будет остановлено", "Вы уверены?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    dbService.StopCreatingTimeSeriesPoints();
                    dbService.RunCreatingTimeSeriesPoints(db.Name, db.Server, db.Timer);
                    db.isMonitored = true;
                    InitDBIsMonitoring();
                    System.Windows.MessageBox.Show($"Извлечение временных рядов из базы данных {db.Name} запущено с периодичностью {db.Timer} минут");

                }
            }
        }

        private void ButtonStruct_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridStruct.Visibility == Visibility.Visible)
            {
                DataGridStruct.Visibility = Visibility.Hidden;
                ButtonStruct.Content = "Показать ▼";
            }
            else {
                DataGridStruct.Visibility = Visibility.Visible;
                ButtonStruct.Content = "Скрыть ▲";
            }
        }
    }
}
