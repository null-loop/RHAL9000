using System;
using System.Collections.Generic;
using RHAL9000.Core;

namespace RHAL9000.Monitors.Builds
{
    public class BuildProjectModel : ModelBase
    {
        private bool _Archived;
        private List<BuildTypeModel> _BuildTypes;
        private string _Description;
        private string _Id;

        private string _Name;

        private Uri _WebUri;

        public string Id
        {
            get { return _Id; }
            set { SetField(ref _Id, value, () => Id); }
        }

        public string Name
        {
            get { return _Name; }
            set { SetField(ref _Name, value, () => Name); }
        }

        public Uri WebUri
        {
            get { return _WebUri; }
            set { SetField(ref _WebUri, value, () => WebUri); }
        }

        public string Description
        {
            get { return _Description; }
            set { SetField(ref _Description, value, () => Description); }
        }

        public bool Archived
        {
            get { return _Archived; }
            set { SetField(ref _Archived, value, () => Archived); }
        }

        public List<BuildTypeModel> BuildTypes
        {
            get { return _BuildTypes; }
            set { SetField(ref _BuildTypes, value, () => BuildTypes); }
        }
    }
}