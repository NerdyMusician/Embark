using Embark.Toolbox;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Embark.Models
{
    public class JobLead : BaseModel
    {
        // Constructors
        public JobLead()
        {
            Position = "";
            Notes = "";
            Employer = "New Employer";
            Links = new();
            Contacts = new();
            Events = new();
            Priority = "Medium";
        }

        // Properties
        private string _Position;
        [XmlSaveMode(XSME.Single)]
        public string Position
        {
            get => _Position;
            set => SetAndNotify(ref _Position, value);
        }

        private string _Notes;
        [XmlSaveMode(XSME.Single)]
        public string Notes
        {
            get => _Notes;
            set => SetAndNotify(ref _Notes, value);
        }

        private string _Employer;
        [XmlSaveMode(XSME.Single)]
        public string Employer
        {
            get => _Employer;
            set => SetAndNotify(ref _Employer, value);
        }

        private ObservableCollection<WebLink> _Links;
        [XmlSaveMode(XSME.Enumerable)]
        public ObservableCollection<WebLink> Links
        {
            get => _Links;
            set => SetAndNotify(ref _Links, value);
        }

        private ObservableCollection<Contact> _Contacts;
        [XmlSaveMode(XSME.Enumerable)]
        public ObservableCollection<Contact> Contacts
        {
            get => _Contacts;
            set => SetAndNotify(ref _Contacts, value);
        }

        private ObservableCollection<Event> _Events;
        [XmlSaveMode(XSME.Enumerable)]
        public ObservableCollection<Event> Events
        {
            get => _Events;
            set => SetAndNotify(ref _Events, value);
        }

        private bool _IsActive;
        [XmlSaveMode(XSME.Single)]
        public bool IsActive
        {
            get => _IsActive;
            set
            {
                _IsActive = value;
                NotifyPropertyChanged();
                Configuration.MainModelRef.UpdateLeadCounts();
                Configuration.MainModelRef.SortJobLeads();
            }
        }

        private string _Priority;
        [XmlSaveMode(XSME.Single)]
        public string Priority
        {
            get => _Priority;
            set
            {
                _Priority = value;
                NotifyPropertyChanged();
                Configuration.MainModelRef.SortJobLeads();
            }
        }

        private bool _HasUpcomingEvent;
        [XmlSaveMode(XSME.Single)]
        public bool HasUpcomingEvent
        {
            get => _HasUpcomingEvent;
            set
            {
                _HasUpcomingEvent = value;
                NotifyPropertyChanged();
                Configuration.MainModelRef.UpdateLeadCounts();
                Configuration.MainModelRef.SortJobLeads();
            }
        }

        private bool _RequiresFollowup;
        [XmlSaveMode(XSME.Single)]
        public bool RequiresFollowup
        {
            get => _RequiresFollowup;
            set
            {
                _RequiresFollowup = value;
                NotifyPropertyChanged();
                Configuration.MainModelRef.UpdateLeadCounts();
                Configuration.MainModelRef.SortJobLeads();
            }
        }

        // Commands
        public ICommand AddLink => new RelayCommand(DoAddLink);
        private void DoAddLink(object param)
        {
            Links.Add(new());
        }
        public ICommand AddContact => new RelayCommand(DoAddContact);
        private void DoAddContact(object param)
        {
            Contacts.Add(new());
        }
        public ICommand AddEvent => new RelayCommand(DoAddEvent);
        private void DoAddEvent(object param)
        {
            Events.Add(new());
        }
        public ICommand RemoveEntry => new RelayCommand(DoRemoveEntry);
        private void DoRemoveEntry(object param)
        {
            Configuration.MainModelRef.JobLeads.Remove(this);
        }

    }
}
