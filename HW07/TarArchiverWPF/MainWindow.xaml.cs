using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using TarArchivator;

namespace TarArchiverWPF
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ArchiveTask> archiveTasks = new ObservableCollection<ArchiveTask>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            taskListView.ItemsSource = archiveTasks;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog folderDialog = new OpenFolderDialog();

            if (folderDialog.ShowDialog() == true)
            {
                var task = new ArchiveTask(folderDialog.FolderName);

                if (!IsValidFolder(task.Folder))
                {
                    MessageBox.Show($"Помилка: Папка '{task.Folder}' не існує або вкладена в іншу вибрану папку.");
                    archiveTasks.Remove(task);
                    return;
                }

                archiveTasks.Add(task);

                try
                {
                    string logFilePath = "archive_log.txt";
                    Archiver archiver = new Archiver(logFilePath);

                    var archiveWindow = new ArchiveWindow(archiver, task);
                    archiveWindow.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка: {ex.Message}");
                    archiveTasks.Remove(task);
                }
            }
        }

        private bool IsValidFolder(string folderPath)
        {
            foreach (ArchiveTask existingTask in archiveTasks)
            {
                if (folderPath.StartsWith(existingTask.Folder) || existingTask.Folder.StartsWith(folderPath))
                {
                    return false;
                }
            }

            return true;
        }
    }
}