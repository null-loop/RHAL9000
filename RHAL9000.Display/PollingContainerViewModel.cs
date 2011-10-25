using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using Caliburn.Micro;
using RHAL9000.Core;

namespace RHAL9000.Display
{
    public class PollingContainerViewModel : Screen, ICalScreen
    {
        public IPollingViewModel ContainedViewModel { get; set; }

        private IObservable<IPollingViewModel> _pollingSource;
        private IDisposable _pollingHandler;

        public PollingContainerViewModel(IPollingViewModel containedViewModel)
        {
            ContainedViewModel = containedViewModel;
        }

        private bool _isPolling;

        public bool IsPolling
        {
            get { return _isPolling; }
            set 
            { 
                _isPolling = value;
                NotifyOfPropertyChange(() => IsPolling);
            }
        }

        private bool _errored;

        public bool Errored
        {
            get { return _errored; }
            set
            {
                _errored = value;
                NotifyOfPropertyChange(() => Errored);
            }
        }

        private string _LastErrorMessage;

        public string LastErrorMessage
        {
            get { return _LastErrorMessage; }
            set 
            { 
                _LastErrorMessage = value;
                NotifyOfPropertyChange(() => LastErrorMessage);
            }
        }

        protected override void OnActivate()
        {
            _pollingSource = Observable.Generate(
                ContainedViewModel,
                b => true,
                b => b,
                b => b,
                b => b.TimeUntilNextPoll);

            _pollingHandler = _pollingSource.Subscribe(b => Poll());

            ThreadPool.QueueUserWorkItem((state) => ActivateContained());

            base.OnActivate();
        }

        private void ActivateContained()
        {
            try
            {

                IsPolling = true;
                ContainedViewModel.Activate();
                IsPolling = false;
                Errored = false;

            }
            catch (Exception ex)
            {
                IsPolling = false;
                Errored = true;
                SetErrorMessage(ex);
            }
        }

        private void Poll()
        {
            try
            {

                IsPolling = true;
                ContainedViewModel.Poll();
                IsPolling = false;
                Errored = false;

            }
            catch (Exception ex)
            {
                IsPolling = false;
                Errored = true;
                SetErrorMessage(ex);
            }
        }

        private void SetErrorMessage(Exception ex)
        {
            LastErrorMessage = string.Format("An unhandled error has occurred in a {0} view. The error message was : \r\n\r\n {1}", ContainedViewModel.GetType().Name, ex.Message);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close && _pollingHandler != null)
            {
                _pollingHandler.Dispose();
            }

            ContainedViewModel.Deactivate(close);
        }

        public void Configure(Dictionary<string, string> dictionary)
        {
            ContainedViewModel.Configure(dictionary);
        }

        public void AddItem(object item)
        {
            throw new NotImplementedException();
        }
    }
}