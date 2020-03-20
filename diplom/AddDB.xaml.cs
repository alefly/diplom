using Service;
using System;
using System.Windows;

namespace Forms
{
    /// <summary>
    /// Логика взаимодействия для AddDB.xaml
    /// </summary>
    public partial class AddDB : Window
    {
        TimeSeriesContext context;
        DBService dbs;
        public AddDB(TimeSeriesContext context)
        {
            InitializeComponent();
            this.context = context;
            dbs = new DBService(context);
        }

        private void ButtonAddBD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ans = dbs.AddElement(TextBoxName.Text, TextBoxServer.Text, TextBoxPort.Text, TextBoxLogin.Text, TextBoxPassword.Password, (int)SliderTimer.Value);
                if (ans)
                {
                    MessageBox.Show("База данных успешно добавлена. Начинаем процесс извлечения временных рядов.");
                }
                else
                {
                    dbs.StopCreatingTimeSeriesPoints();
                    dbs.RunCreatingTimeSeriesPoints(TextBoxName.Text, TextBoxServer.Text, (int)SliderTimer.Value);
                    MessageBox.Show("База данных уже имеется в системе. Начинаем процесс извлечения временных рядов.");
                }
                var form = new MainWindow(context);
                form.Show();
                Close();
            }
            catch (Exception ex) {
                MessageBox.Show("Проверьте правильность введенных данных", "Ошибка");
            }
        }
    }
}
