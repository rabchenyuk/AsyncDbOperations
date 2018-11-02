using AsyncLoadToDB.Infrastructure;
using DBLibrary.EF.Context;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WorkWithTask;

namespace AsyncLoadToDB.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private TestEntityProvider _testProvider;
        private ManualResetEvent _locker = new ManualResetEvent(true);

        private int counter;

        private bool isENabledCancel;
        public bool IsEnabledCancel
        {
            get { return isENabledCancel; }
            set
            {
                isENabledCancel = value;
                OnPropertyChanged("IsEnabledCancel");
            }
        }

        private bool isEnabledPause;
        public bool IsEnabledPause
        {
            get { return isEnabledPause; }
            set
            {
                isEnabledPause = value;
                OnPropertyChanged("IsEnabledPause");
            }
        }

        private bool isEnabledStopAndSave;

        public bool IsEnabledStopAndSave
        {
            get { return isEnabledStopAndSave; }
            set
            {
                isEnabledStopAndSave = value;
                OnPropertyChanged("IsEnabledStopAndSave");
            }
        }

        public MainWindowViewModel()
        {
            ConnectionProvider connProvider = new ConnectionProvider();
            connProvider.ConectedEvent += ConnectionCompleted;
            connProvider.ConnectRun();
        }

        private void ConnectionCompleted(AsyncLoadEntity context)
        {
            _testProvider = new TestEntityProvider(context);
            _testProvider.AddTestEntityEvent += _testProvider_AddTestEntityEvent;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ConnComplete = "Connection success";
            }));
        }

        private string connComplete;
        public string ConnComplete
        {
            get
            {
                if (string.IsNullOrEmpty(connComplete))
                    connComplete = "Waiting for a connection";
                return connComplete;
            }
            set
            {
                connComplete = value;
                OnPropertyChanged("ConnComplete");
            }
        }

        private string count;
        public string Count
        {
            get
            {
                if (string.IsNullOrEmpty(count))
                    count = "0/0";
                return count;
            }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        private string countNotes;
        public string CountNotes
        {
            get
            {
                return countNotes;
            }
            set
            {
                countNotes = value;
                OnPropertyChanged("CountNotes");
            }
        }

        private string pauseBtn;
        public string PauseBtn
        {
            get
            {
                if (string.IsNullOrEmpty(pauseBtn))
                    pauseBtn = "Pause";
                return pauseBtn;
            }
            set
            {
                pauseBtn = value;
                OnPropertyChanged("PauseBtn");
            }
        }


        #region Command
        RelayCommand AddCommand;
        RelayCommand PauseCommand;
        RelayCommand CancelCommand;
        RelayCommand StopAndSaveCommand;

        public ICommand StopAndSave
        {
            get
            {
                if (StopAndSaveCommand == null)
                    StopAndSaveCommand = new RelayCommand(ExecuteStopAndSaveCommand, CanExecuteStopAndSaveCommand);
                return StopAndSaveCommand;
            }
        }

        private void ExecuteStopAndSaveCommand(object obj)
        {
            _testProvider.StopAndSaveFlag = true;

            CountNotes = string.Empty;
            IsEnabledPause = false;
            IsEnabledCancel = false;
            IsEnabledStopAndSave = false;
            count = null;
        }

        private bool CanExecuteStopAndSaveCommand(object obj)
        {
            return true;
        }

        public ICommand Cancel
        {
            get
            {
                if (CancelCommand == null)
                    CancelCommand = new RelayCommand(ExecuteCancelCommand, CanExecuteCancelCommand);
                return CancelCommand;
            }
        }

        private void ExecuteCancelCommand(object obj)
        {
            CountNotes = null;
            _testProvider.CancelFlag = true;
            IsEnabledCancel = false;
            IsEnabledPause = false;
            IsEnabledStopAndSave = false;
        }

        private bool CanExecuteCancelCommand(object obj)
        {
            return true;
        }

        public ICommand Pause
        {
            get
            {
                if (PauseCommand == null)
                    PauseCommand = new RelayCommand(ExecutePauseCommand, CanExecutePauseCommand);
                return PauseCommand;
            }
        }

        private void ExecutePauseCommand(object obj)
        {
            if(PauseBtn == "Pause")
            {
                _locker.Reset();
                PauseBtn = "Continue";

                IsEnabledStopAndSave = false;
                IsEnabledCancel = false;
            }
            else
            {
                _locker.Set();
                PauseBtn = "Pause";
                IsEnabledStopAndSave = true;
                IsEnabledCancel = true;
            }
        }

        private bool CanExecutePauseCommand(object obj)
        {
            return true;
        }

        public ICommand AddNotes
        {
            get
            {
                if (AddCommand == null)
                    AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand);
                return AddCommand;
            }
        }

        private void ExecuteAddCommand(object obj)
        {
            IsEnabledCancel = true;
            IsEnabledPause = true;
            IsEnabledStopAndSave = true;

            _testProvider.AddTestEntityRangeAsync(Convert.ToInt32(CountNotes));

            counter = Convert.ToInt32(CountNotes);
            CountNotes = null;
        }

        private bool CanExecuteAddCommand(object obj)
        {
            if (string.IsNullOrEmpty(CountNotes))
                return false;
            return true;
        }
        #endregion

        private void _testProvider_AddTestEntityEvent(int count)
        {
            _locker.WaitOne();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Count = $"{Convert.ToString(count)}/{counter}";
            }));

            if (count == counter)
            {
                IsEnabledCancel = false;
                IsEnabledPause = false;
                IsEnabledStopAndSave = false;
            }
        }
    }
}
