using System;
using RHAL9000.Core;

namespace RHAL9000.Monitors.Builds
{
    public class BuildModel : ModelBase
    {
        private string _agentId;
        private BuildTypeModel _buildType;
        private string _currentStage;
        private TimeSpan _elapsedRunTime;
        private TimeSpan? _estimatedRunTime;
        private DateTime? _finished;
        private bool _history;
        private string _id;
        private bool _isRunning;

        private string _number;
        private bool? _outDated;
        private decimal? _percentageComplete;
        private bool _personal;
        private bool _pinned;
        private bool _probablyHanging;
        private DateTime _started;

        private BuildStatus _status;
        private string _statusText;

        private Uri _webUri;

        public string Id
        {
            get { return _id; }
            set { SetField(ref _id, value, () => Id); }
        }

        public string Number
        {
            get { return _number; }
            set { SetField(ref _number, value, () => Number); }
        }

        public BuildStatus Status
        {
            get { return _status; }
            set { SetField(ref _status, value, () => Status); }
        }

        public Uri WebUri
        {
            get { return _webUri; }
            set { SetField(ref _webUri, value, () => WebUri); }
        }

        public bool Personal
        {
            get { return _personal; }
            set { SetField(ref _personal, value, () => Personal); }
        }

        public bool History
        {
            get { return _history; }
            set { SetField(ref _history, value, () => History); }
        }

        public bool Pinned
        {
            get { return _pinned; }
            set { SetField(ref _pinned, value, () => Pinned); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetField(ref _statusText, value, () => StatusText); }
        }

        public BuildTypeModel BuildType
        {
            get { return _buildType; }
            set { SetField(ref _buildType, value, () => BuildType); }
        }

        public DateTime Started
        {
            get { return _started; }
            set { SetField(ref _started, value, () => Started); }
        }

        public DateTime? Finished
        {
            get { return _finished; }
            set { SetField(ref _finished, value, () => Finished); }
        }

        public string AgentId
        {
            get { return _agentId; }
            set { SetField(ref _agentId, value, () => AgentId); }
        }

        public decimal? PercentageComplete
        {
            get { return _percentageComplete; }
            set { SetField(ref _percentageComplete, value, () => PercentageComplete); }
        }

        public TimeSpan ElapsedRunTime
        {
            get { return _elapsedRunTime; }
            set { SetField(ref _elapsedRunTime, value, () => ElapsedRunTime); }
        }

        public TimeSpan? EstimatedRunTime
        {
            get { return _estimatedRunTime; }
            set { SetField(ref _estimatedRunTime, value, () => EstimatedRunTime); }
        }

        public string CurrentStage
        {
            get { return _currentStage; }
            set { SetField(ref _currentStage, value, () => CurrentStage); }
        }

        public bool? OutDated
        {
            get { return _outDated; }
            set { SetField(ref _outDated, value, () => OutDated); }
        }

        public bool ProbablyHanging
        {
            get { return _probablyHanging; }
            set { SetField(ref _probablyHanging, value, () => ProbablyHanging); }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetField(ref _isRunning, value, () => IsRunning); }
        }
    }
}