using System.Windows;

namespace task3
{
    public partial class App : Application
    {
        private static Semaphore _semaphore = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "WpfApp";

            _semaphore = new Semaphore(3, 3, appName);

            if (!_semaphore.WaitOne(0))
            {
                MessageBox.Show("Додаток може бути запущений тільки в трьох копіях. Закриття четвертої копії");
                Application.Current.Shutdown();
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _semaphore.Release();
            base.OnExit(e);
        }
    }
}