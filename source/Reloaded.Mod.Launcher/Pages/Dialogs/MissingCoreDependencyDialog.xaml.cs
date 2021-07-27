﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Reloaded.Mod.Launcher.Utility;
using Reloaded.Mod.Loader.IO.Utility;
using Reloaded.Mod.Loader.Update.Dependency;
using Reloaded.Mod.Loader.Update.Dependency.Interfaces;
using Reloaded.WPF.Theme.Default;

namespace Reloaded.Mod.Launcher.Pages.Dialogs
{
    /// <summary>
    /// Interaction logic for MissingCoreDependencyDialog.xaml
    /// </summary>
    public partial class MissingCoreDependencyDialog : ReloadedWindow
    {
        public new MissingCoreDependencyDialogViewModel ViewModel { get; set; }

        public MissingCoreDependencyDialog(DependencyChecker deps)
        {
            ViewModel = new MissingCoreDependencyDialogViewModel(deps);
            InitializeComponent();
        }

        private async void DownloadButtonClick(object sender, RoutedEventArgs e)
        {
            var model = (DependencyModel)((FrameworkElement)sender).DataContext;
            await model.OpenUrlsAsync();
        }
    }

    public class MissingCoreDependencyDialogViewModel : ObservableObject
    {
        public ObservableCollection<DependencyModel> Dependencies { get; set; }

        public MissingCoreDependencyDialogViewModel(DependencyChecker deps)
        {
            Dependencies = new ObservableCollection<DependencyModel>(deps.Dependencies.Where(x => !x.Available).Select(x => new DependencyModel(x)));
        }
    }

    public class DependencyModel
    {
        public string Name    => Dependency.Name;
        public bool IsMissing => !Dependency.Available;
        public IDependency Dependency;

        public DependencyModel(IDependency dependency)
        {
            Dependency = dependency;
        }

        public async Task OpenUrlsAsync()
        {
            foreach (var url in await Dependency.GetUrlsAsync()) 
                ProcessExtensions.OpenFileWithDefaultProgram(url);
        }
    }
}
