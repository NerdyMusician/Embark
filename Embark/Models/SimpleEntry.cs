namespace Embark.Models
{
    public class SimpleEntry : BaseModel
    {
        public SimpleEntry()
        {
            Label = "";
            Text = "";
            Description = "";
        }

        private string _Label;
        public string Label
        {
            get => _Label;
            set => SetAndNotify(ref _Label, value);
        }
        private string _Text;
        public string Text
        {
            get => _Text;
            set => SetAndNotify(ref _Text, value);
        }
        private string _Description;
        public string Description
        {
            get => _Description;
            set => SetAndNotify(ref _Description, value);
        }

    }
}
