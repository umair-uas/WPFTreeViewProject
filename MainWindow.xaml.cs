using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;



namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// 
        public MainWindow()
        {
            InitializeComponent();

          
           
        }
        #endregion


        #region On Loaded

        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Get every logical drive on the machine
           foreach (var drive in  Directory.GetLogicalDrives())
           {
               // Create a new item for it
               var item = new TreeViewItem()
               {
                   //set the header 
                   Header = drive,

                   //and  the full path path
                   Tag = drive

               };

      
               // add a dummy item
               item.Items.Add(null);

               //Listen out for item being expanded 
               item.Expanded += Folder_Expanded;

               //Add it to the main treeview
               FolderView.Items.Add(item);
           }
        }
        #endregion 

        #region Folder Expanded

    /// <summary>
    /// When a folder is expanded, find the subfolder/files
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
       

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks
            var item = (TreeViewItem)sender;

            // If the item only contains the dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;
            // clear the dummy data

            item.Items.Clear();

            // Get full path 

            var fullPath = (string)item.Tag;

            #endregion 

            #region Get Folders
            // Create a blank list for directories 
            var directories = new List<string>();
            // Try and get directories from the folder
            // ignoring any issues doing so 

            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                    directories.AddRange(dirs);

              
            }
            catch { }
            
            // for each directory
            directories.ForEach(directoryPath =>
                {
                    //Create directory Item
                    var subItem=new TreeViewItem()
                    {
                        // set header as folder name and tag as full path 
                        Header=GetFileForlderName(directoryPath),
                        Tag=directoryPath
                    };

                    // add dummy item so we can expand folder 
                    subItem.Items.Add(null);
                    // Handle expanding
                     subItem.Expanded += Folder_Expanded;

                    // add this item to the parent 
                     item.Items.Add(subItem);
                });
            #endregion 

            #region Get Files 
            // Create a blank list for files 
            var files = new List<string>();
            // Try and get files from the folder
            // ignoring any issues doing so 

            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);

              
            }
            catch { }
            
            // for each file
            files.ForEach(filepath =>
                {
                    //Create file Item
                    var subItem=new TreeViewItem()
                    {
                        // set header as file name and tag as full path 
                        Header=GetFileForlderName(filepath),
                        Tag=filepath
                    };

                    // add this item to the parent 
                    item.Items.Add(subItem);
                });

            #endregion 


        }
        #endregion

        #region Helpers
        /// <summary>
        /// Find the file or folder name from full path 
        /// </summary>
        /// <param name="path"> The full path </param>
        /// <returns></returns>
        public static string GetFileForlderName(string path){
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            // Make all slashes back slashes 
            var normalizedpath = path.Replace('/', '\\');
            // find the last backslash in the path
            var lastIndex = normalizedpath.LastIndexOf('\\');

            // if we don't find a backslash ,return the path itself
            if (lastIndex <= 0)
                return path;
            // return the name after the last back slash 
            return path.Substring(lastIndex + 1);
        }
        #endregion

    }
}
