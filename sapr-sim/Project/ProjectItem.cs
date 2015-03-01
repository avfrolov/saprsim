using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace sapr_sim
{
    public class ProjectItem
    {

        private Canvas canvas;
        private string name;

        public ProjectItem()
        {
            // for xml serialization only
        }

        public ProjectItem(string name)
        {
            this.name = name;
        }

        public ProjectItem(Canvas canvas, string name)
        {
            this.canvas = canvas;
            this.name = name;
        }

        [XmlIgnore]
        public Canvas Canvas
        {
            get { return canvas; }
            set { canvas = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string FullPath
        {
            get { return Project.Instance.FullPath + "\\" + name + FileService.PROJECT_ITEM_EXTENSION; }
        }
    }
}
