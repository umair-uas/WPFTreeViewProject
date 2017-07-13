using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfTreeView
{
    /// <summary>
    /// Converts a full path to a specific image type of a drive,folder or file
    /// </summary>
    
   [ValueConversion(typeof(string),typeof(BitmapImage))]
   public   class HeaderToImageConverter: IValueConverter
    {
       public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the full path 
            var path = (string)value;
            // if the path is null 
            if (path == null)
                return null;
             // Get the name of the file/folder
            var name = MainWindow.GetFileForlderName(path);

            // By default, we presume an image
            var newimage = "Images/fileimage.png";
            
            // If the name is blank, we presume it's a drive as we cannot have a blank file or folder name
            if(string.IsNullOrEmpty(name))
                newimage = "Images/CDrive.png";
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                newimage = "Images/folderclosed.png";

            return new BitmapImage(new Uri("pack://application:,,,/Images/folderclosed.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
