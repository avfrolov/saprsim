using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sapr_sim.Utils
{
    public class FileService
    {
        public ScrollableCanvas open(string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                return (ScrollableCanvas) new BinaryFormatter().Deserialize(fs);
            }
        }

        public void save(Canvas currentCanvas, string filepath)
        {
            using (FileStream filestream = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                new BinaryFormatter().Serialize(filestream, currentCanvas);
            }
        }

    }
}
