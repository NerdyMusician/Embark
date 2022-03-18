using Embark.Toolbox;
using System.Windows.Input;

namespace Embark.Models
{
    public class Event : BaseModel
    {
        // Constructors
        public Event()
        {

        }

        // Properties
        private string _Name;
        [XmlSaveMode(XSME.Single)]
        public string Name
        {
            get => _Name;
            set => SetAndNotify(ref _Name, value);
        }

        private string _DateAndTime;
        [XmlSaveMode(XSME.Single)]
        public string DateAndTime
        {
            get => _DateAndTime;
            set => SetAndNotify(ref _DateAndTime, value);
        }

        private string _Notes;
        [XmlSaveMode(XSME.Single)]
        public string Notes
        {
            get => _Notes;
            set => SetAndNotify(ref _Notes, value);
        }

        // Commands
        public ICommand RemoveEvent => new RelayCommand(DoRemoveEvent);
        private void DoRemoveEvent(object param)
        {
            Configuration.MainModelRef.ActiveJobLead.Events.Remove(this);
        }

    }
}
