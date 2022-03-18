using Embark.Toolbox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;

namespace Embark.Models.ViewModels
{
    public class MainViewModel : BaseModel
    {
        public MainViewModel()
        {
            Configuration.MainModelRef = this;
            PriorityLevels = new() { "High", "Medium", "Low" };

            XmlMethods.XmlToList("Data/JobLeads.xml", out List<JobLead> items);
            JobLeads = new ObservableCollection<JobLead>(items);

            ApplicationVersion = "Embark 1.01.00 beta";

            SortJobLeads();

        }

        private List<string> _PriorityLevels;
        public List<string> PriorityLevels
        {
            get => _PriorityLevels;
            set => SetAndNotify(ref _PriorityLevels, value);
        }

        private string _ApplicationVersion;
        public string ApplicationVersion
        {
            get => _ApplicationVersion;
            set => SetAndNotify(ref _ApplicationVersion, value);
        }
        private ObservableCollection<JobLead> _JobLeads;
        public ObservableCollection<JobLead> JobLeads
        {
            get => _JobLeads;
            set => SetAndNotify(ref _JobLeads, value);
        }

        private JobLead _ActiveJobLead;
        public JobLead ActiveJobLead
        {
            get => _ActiveJobLead;
            set => SetAndNotify(ref _ActiveJobLead, value);
        }

        private int _TotalLeadCount;
        public int TotalLeadCount
        {
            get => _TotalLeadCount;
            set => SetAndNotify(ref _TotalLeadCount, value);
        }
        private int _PriorityLeadCount;
        public int PriorityLeadCount
        {
            get => _PriorityLeadCount;
            set => SetAndNotify(ref _PriorityLeadCount, value);
        }
        private int _ActiveLeadCount;
        public int ActiveLeadCount
        {
            get => _ActiveLeadCount;
            set => SetAndNotify(ref _ActiveLeadCount, value);
        }
        private int _DeadLeadCount;
        public int DeadLeadCount
        {
            get => _DeadLeadCount;
            set => SetAndNotify(ref _DeadLeadCount, value);
        }

        // Commands
        public ICommand AddJob => new RelayCommand(DoAddJobLead);
        private void DoAddJobLead(object param)
        {
            JobLead jobLead = new();
            JobLeads.Add(jobLead);
            ActiveJobLead = jobLead;
            UpdateLeadCounts();
        }
        public ICommand SortJobs => new RelayCommand(DoSortJobLeads);
        private void DoSortJobLeads(object param)
        {
            if (!Configuration.LoadComplete) { return; }
            JobLeads = new(JobLeads
                .OrderBy(j => !j.HasUpcomingEvent)
                .ThenBy(j => !j.RequiresFollowup)
                .ThenBy(j => !j.IsActive)
                .ThenBy(j => (j.Priority switch { "High" => 0, "Medium" => 1, "Low" => 2, _ => 3 }))
                .ThenBy(j => j.Employer));
        }
        public ICommand SaveJobs => new RelayCommand(DoSaveJobs);
        private void DoSaveJobs(object param)
        {
            try
            {
                if (JobLeads.Count == 0)
                {
                    // Prevents zero item save crash
                    XDocument blankDoc = new();
                    blankDoc.Add(new XElement("JobLeadSet"));
                    blankDoc.Save("Data/JobLeads.xml");
                    AuxMethods.NotifyUser($"{JobLeads.Count}Job Leads Saved.");
                    return;
                }
                XDocument itemDocument = new();
                itemDocument.Add(XmlMethods.ListToXml(JobLeads.ToList()));
                itemDocument.Save("Data/JobLeads.xml");
                AuxMethods.NotifyUser($"{JobLeads.Count} Job Leads Saved.");
            }
            catch (Exception e)
            {
                AuxMethods.NotifyUser(e.Message);
            }
        }
        public ICommand ProcessKeyboardShortcut => new RelayCommand(DoProcessKeyboardShortcut);
        private void DoProcessKeyboardShortcut(object key)
        {
            switch (key.ToString())
            {
                case "CtrlS":
                    SaveJobs.Execute(null);
                    break;
                case "CtrlN":
                    AddJob.Execute(null);
                    break;
                default:
                    break;
            }
        }

        // Public Methods
        public void UpdateLeadCounts()
        {
            if (!Configuration.LoadComplete) { return; }
            int total = 0;
            int priority = 0;
            int active = 0;
            int dead = 0;
            foreach (JobLead lead in JobLeads)
            {
                total++;
                if (!lead.IsActive) { dead++; continue; }
                if (lead.HasUpcomingEvent) { priority++; continue; }
                active++;
            }
            TotalLeadCount = total;
            PriorityLeadCount = priority;
            ActiveLeadCount = active;
            DeadLeadCount = dead;
        }
        public void SortJobLeads()
        {
            DoSortJobLeads(null);
        }

    }
}
