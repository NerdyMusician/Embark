using Embark.Toolbox;
using System.Windows.Input;

namespace Embark.Models
{
    public class Contact : BaseModel
    {
        // Constructors
        public Contact()
        {
            Name = "New Contact";
            Role = "";
            Email = "";
            Phone = "";
        }

        // Properties
        private string _Name;
        [XmlSaveMode(XSME.Single)]
        public string Name
        {
            get => _Name;
            set => SetAndNotify(ref _Name, value);
        }

        private string _Role;
        [XmlSaveMode(XSME.Single)]
        public string Role
        {
            get => _Role;
            set => SetAndNotify(ref _Role, value);
        }

        private string _Email;
        [XmlSaveMode(XSME.Single)]
        public string Email
        {
            get => _Email;
            set => SetAndNotify(ref _Email, value);
        }

        private string _Phone;
        [XmlSaveMode(XSME.Single)]
        public string Phone
        {
            get => _Phone;
            set => SetAndNotify(ref _Phone, value);
        }

        private bool _InExpandedView;
        public bool InExpandedView
        {
            get => _InExpandedView;
            set => SetAndNotify(ref _InExpandedView, value);
        }

        // Commands
        public ICommand RemoveContact => new RelayCommand(DoRemoveContact);
        private void DoRemoveContact(object param)
        {
            Configuration.MainModelRef.ActiveJobLead.Contacts.Remove(this);
        }

    }
}
