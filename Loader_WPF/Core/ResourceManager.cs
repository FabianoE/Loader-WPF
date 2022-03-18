using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Loader_WPF.Core
{
    public class ResourceManager
    {
        static internal ImageSource GetImageFromResource(string ResourceName)
        {
            Uri oUri = new Uri("pack://application:,,,/Loader_WPF;component/" + ResourceName, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(oUri);
        }
    }
}
