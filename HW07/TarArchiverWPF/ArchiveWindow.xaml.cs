using System.Windows;
using TarArchivator;

namespace TarArchiverWPF
{
    public partial class ArchiveWindow : Window
    {
        private Archiver _archiver;
        private ArchiveTask _archiveTask;

        public ArchiveWindow(Archiver archiver, ArchiveTask archiveTask)
        {
            InitializeComponent();
            DataContext = this;

            Title = $"Archive Window - {archiveTask.Folder}";

            this._archiver = archiver;
            this._archiver.LogUpdated += LogUpdatedHandler;
            this._archiver.ProgressUpdated += ProgressUpdateHandler;

            this._archiveTask = archiveTask;
            taskListView.ItemsSource = new List<ArchiveTask> { this._archiveTask };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var arTask = new ArchiveTask(this._archiveTask.Folder);

            var thread = new Thread(() => ArchiveFolderAsync(arTask).Wait());
            thread.Start();
        }

        private async Task ArchiveFolderAsync(ArchiveTask task)
        {
            try
            {
                await this._archiver.CreateArchiveAsync($"{task.Folder}.tar.gz", task.Folder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка архівації: {ex.Message}");
            }
        }

        private void LogUpdatedHandler(string text)
        {
            Dispatcher.Invoke(() => { logTextBlock.AppendText(text); });
        }

        private void ProgressUpdateHandler(string text)
        {
            Dispatcher.Invoke(() => { this._archiveTask.Progress = text; });
        }
    }
}