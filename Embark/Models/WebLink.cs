using Embark.Toolbox;
using System;
using System.Windows.Input;

namespace Embark.Models
{
    public class WebLink : BaseModel
    {
        // Constructors
        public WebLink()
        {
            Url = "";
        }

        // Properties
        private string _Url;
        [XmlSaveMode(XSME.Single)]
        public string Url
        {
            get => _Url;
            set => SetAndNotify(ref _Url, value);
        }

        // Commands
        public ICommand RemoveLink => new RelayCommand(DoRemoveLink);
        private void DoRemoveLink(object param)
        {
            Configuration.MainModelRef.ActiveJobLead.Links.Remove(this);
        }
        public ICommand OpenUrl => new RelayCommand(DoOpenUrl);
        private void DoOpenUrl(object param)
        {
            try
            {
                if (string.IsNullOrEmpty(Url))
                {
                    AuxMethods.NotifyUser("Url is blank.");
                    return;
                }
                System.Diagnostics.Process.Start("explorer", Url);
            }
            catch (Exception e)
            {
                AuxMethods.NotifyUser(e.Message);
            }
        }

    }
}
