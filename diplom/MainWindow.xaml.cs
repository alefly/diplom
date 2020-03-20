using Service;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Button = System.Windows.Controls.Button;

namespace Forms
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSeriesContext context;
        private ViewDB currentDB;
        private DBService dbs;
        private System.Threading.Timer timer;
        private DBRequests dbRequests;
        public MainWindow()
        {
            InitializeComponent();
            context = new TimeSeriesContext();
            InitializationComponents();
        }

        private void InitializationComponents() {
            dbRequests = new DBRequests(context);
            dbs = new DBService(context);
            var ans = context.DBs.FirstOrDefault(rec => rec.isMonitored == true);
            if (ans == null) {
                InitCurrDbIsnt();
            }
            else
            {
                InitCurrDbIs(ans.Id);      
            }
            RefreshDBList(dbs.GetList());
            timer = new System.Threading.Timer(f => { Refresh(); }, null, 0, 1 * 60 * 1000);
        }

        private void InitCurrDbIsnt() {
            currentDB = null;
            buttonStop.IsEnabled = false;
            LabelName.Content = "База данных для извлечения временных рядов не выбрана";
            LabelTimer.Content = "Добавьте новую базу данных в меню или выберете базу данных из списка ниже";
            LabelLastCheck.Content = "";
            buttonCurrentDB.Visibility = Visibility.Hidden;
        }

        private void InitCurrDbIs(int dbId) {
            currentDB = dbs.GetElement(dbId);
            buttonStop.IsEnabled = true;
            LabelName.Content = $"База данных для извлечения временных рядов: {currentDB.Name}";
            LabelTimer.Content = $"Регулярность: {currentDB.Timer} минут";
            LabelLastCheck.Content = $"последняя проверка: {currentDB.LastCheck.ToShortDateString()} {currentDB.LastCheck.ToShortTimeString()}";
            buttonCurrentDB.Visibility = Visibility.Visible;
        }

        private void Refresh() {
            Dispatcher.Invoke(() =>
            {
                var ans = context.DBs.FirstOrDefault(rec => rec.isMonitored == true);
                if (ans == null && currentDB == null)
                    return;
                if (ans == null)
                {
                    InitCurrDbIsnt();
                }
                else 
                {
                    ViewDB currdb = dbs.GetElement(ans.Id);
                    TimeSpan dif = DateTime.Now.Subtract(currdb.LastCheck);
                    if (Math.Abs(dif.TotalMinutes) > currdb.Timer)
                    {
                        dbRequests.RequestsCreateTSPEntities(currdb);
                        context.DBs.FirstOrDefault(rec => rec.Id == currdb.Id).LastCheck = DateTime.Now;
                        context.SaveChanges();
                        System.Windows.MessageBox.Show("Точка создана");
                    }
                    InitCurrDbIs(ans.Id);
                }
            });
        }

        public MainWindow(TimeSeriesContext context)
        {
            InitializeComponent();
            this.context = context;
            InitializationComponents();
        }

        public void RefreshDBList(List<ViewDB> listDbs)
        {
            List<Button> list = new List<Button>();
            foreach (var db in listDbs)
            {
                Button element = new Button();
                element.Height = 30;
                element.Width = listBoxDB.Width - 8;
                element.Content = db.ToString();
                element.Click += (object sender, RoutedEventArgs e) => {
                    var form = new VisionDB(context, db.Id);
                    form.Show();
                };
                list.Add(element);
            }
            listBoxDB.ItemsSource = list;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddDB(context);
            form.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы уверены, что хотите остановить извлечение временных рядов для текущей базы данных?", "Остановка извлечения", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                dbs.StopCreatingTimeSeriesPoints();
                InitCurrDbIsnt();
            }
        }

        private void buttonCurrentDB_Click(object sender, RoutedEventArgs e)
        {
            var form = new VisionDB(context, currentDB.Id);
            form.Show();
        }
    }
}
