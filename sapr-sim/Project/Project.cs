using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sapr_sim
{
    public class Project
    {

        private string projectName;
        private string projectPath;

        private List<ProjectItem> items = new List<ProjectItem>();

        private static Project instance = new Project();

        private Project() { }

        public static Project Instance
        {
            get { return instance; }
        }

        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        public string ProjectPath
        {
            get { return projectPath; }
            set { projectPath = value; }
        }

        public string FullPath
        {
            get { return projectPath + "\\" + projectName; }
        }

        public bool IsLoaded
        {
            get { return !String.IsNullOrEmpty(projectPath) && !String.IsNullOrEmpty(projectName); }
        }

        public List<ProjectItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public void addProjectItem(ProjectItem item)
        {
            items.Add(item);
        }

        public void removeProjectItem(ProjectItem item)
        {
            items.Remove(item);
        }

        public ProjectItem byCanvas(Canvas canvas)
        {
            return items.Where(i => i.Canvas.Equals(canvas)).First();
        }

        public void Close()
        {
            items.Clear();
            projectName = "";
            projectPath = "";
        }
    }
}
