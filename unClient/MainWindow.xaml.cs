using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net.Sockets;



namespace unClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            const int bufferSize = 1024;
            Cursor currentCursor = this.Cursor;
            this.Cursor = Cursors.Wait;
            string serverName = Properties.Settings.Default.Server;
            int port = Properties.Settings.Default.Port;

            var client = new TcpClient();
            NetworkStream stream = null;
            try
            {
                client.Connect(serverName, port);

                stream = client.GetStream();
                byte[] buffer = new byte[bufferSize];


                stream.Write(buffer, 0, bufferSize);




                txt_Result.Text = Encoding.Unicode.GetString(buffer).Trim('\0');

                MessageBox.Show(Encoding.Unicode.GetString(buffer).Trim('\0'),"Получен результат", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка запроса к серверу", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (client.Connected)
                {
                    client.Close();
                }
            }
            this.Cursor = currentCursor;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


//---- Код для сворачивания в трей! ---------------------------------------------------------

        private System.Windows.Forms.NotifyIcon TrayIcon = null;
        private ContextMenu TrayMenu = null;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e); // базовый функционал приложения в момент запуска
            createTrayIcon(); // создание нашей иконки
        }

        private bool createTrayIcon()
        {
            bool result = false;
            if (TrayIcon == null)
            { // только если мы не создали иконку ранее
                TrayIcon = new System.Windows.Forms.NotifyIcon(); // создаем новую
                TrayIcon.Icon = unClient.Properties.Resources.Icon1; // изображение для трея
                // обратите внимание, за ресурсом с картинкой мы лезем в свойства проекта, а не окна,
                // поэтому нужно указать полный namespace
                TrayIcon.Text = "Here is tray icon text."; // текст подсказки, всплывающей над иконкой
                TrayMenu = Resources["TrayMenu"] as ContextMenu; // а здесь уже ресурсы окна и тот самый x:Key

                // сразу же опишем поведение при щелчке мыши, о котором мы говорили ранее
                // это будет просто анонимная функция, незачем выносить ее в класс окна
                TrayIcon.Click += delegate(object sender, EventArgs e)
                {
                    if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        // по левой кнопке показываем или прячем окно
                        ShowHideMainWindow(sender, null);
                    }
                    else
                    {
                        // по правой кнопке (и всем остальным) показываем меню
                        TrayMenu.IsOpen = true;
                        Activate(); // нужно отдать окну фокус, см. ниже
                    }
                };
                result = true;
            }
            else
            { // все переменные были созданы ранее
                result = true;
            }
            TrayIcon.Visible = true; // делаем иконку видимой в трее
            return result;
        }


        private void ShowHideMainWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false; // спрячем менюшку, если она вдруг видима
            if (IsVisible)
            {// если окно видно на экране
                // прячем его
                Hide();
                // меняем надпись на пункте меню
                (TrayMenu.Items[0] as MenuItem).Header = "Show";
            }
            else
            { // а если не видно
                // показываем
                Show();
                // меняем надпись на пункте меню
                (TrayMenu.Items[0] as MenuItem).Header = "Hide";
                WindowState = CurrentWindowState;
                Activate(); // обязательно нужно отдать фокус окну,
                // иначе пользователь сильно удивится, когда увидит окно
                // но не сможет в него ничего ввести с клавиатуры
            }
        }

        private WindowState fCurrentWindowState = WindowState.Normal;
        public WindowState CurrentWindowState
        {
            get { return fCurrentWindowState; }
            set { fCurrentWindowState = value; }
        }

        // переопределяем встроенную реакцию на изменение состояния сознания окна
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e); // системная обработка
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                // если окно минимизировали, просто спрячем
                Hide();
                // и поменяем надпись на менюшке
                (TrayMenu.Items[0] as MenuItem).Header = "Show";
            }
            else
            {
                // в противном случае запомним текущее состояние
                CurrentWindowState = WindowState;
            }
        }


        private bool fCanClose = false;
        public bool CanClose
        { // флаг, позволяющий или запрещающий выход из приложения
            get { return fCanClose; }
            set { fCanClose = value; }
        }

        // переопределяем обработчик запроса выхода из приложения
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e); // встроенная обработка
            if (!CanClose)
            {    // если нельзя закрывать
                e.Cancel = true; //выставляем флаг отмены закрытия
                // запоминаем текущее состояние окна
                CurrentWindowState = this.WindowState;
                // меняем надпись в менюшке
                (TrayMenu.Items[0] as MenuItem).Header = "Show";
                // прячем окно
                Hide();
            }
            else
            { // все-таки закрываемся
                // убираем иконку из трея
                TrayIcon.Visible = false;
            }
        }

        private void MenuExitClick(object sender, RoutedEventArgs e)
        {
            CanClose = true;
            Close();
        }

    }
}
