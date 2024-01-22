using System.ComponentModel;

namespace TarArchivator
{
    public class ArchiveTask : INotifyPropertyChanged
    {
        private string _folder;
        public string Folder
        {
            get { return _folder; }
            set
            {
                if (_folder != value)
                {
                    _folder = value;
                    OnPropertyChanged(nameof(Folder));
                }
            }
        }

        private string _progress;
        public string Progress
        {
            get { return _progress; }
            set
            {
                if (value != _progress)
                {
                    _progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        public ArchiveTask(string folder)
        {
            Folder = folder;
            Progress = "Not Started";
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
