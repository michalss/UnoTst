using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPlayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnoTST.Models;
using UnoTST.Services;
using Windows.Storage;

namespace UnoTST.ViewsModel
{
    public sealed partial class HttpTestingViewModel : BaseViewModel
    {

        private HttpService _httpService { get; set; }
        private IDialogService _dialog { get; set; }
       
        public HttpTestingViewModel(HttpService httpService, IDialogService dialog)
        {
            _httpService = httpService;
            _dialog = dialog;


           LoadRepository();
        }



        [RelayCommand]
        public async Task AddRepository()
        {
            var repositoryUrl = "https://mplayer.cz/repo.json"; //await _dialog.InputStringDialogAsync("New Repo", "https://mplayer.cz/repo.json", "Add", "Cancel");

            if (string.IsNullOrEmpty(repositoryUrl) == false)
            {
                //var lister = _settings.ReadStringValue("repository_list");

                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                var lister = localSettings.Values["repository_list"] as string;
                var repo = new List<MRepository>();
                if (!String.IsNullOrEmpty(lister))
                {
                    repo = JsonSerializer.Deserialize<List<MRepository>>(lister);
                }

                //need to load repository from url ??
                var newList = await _httpService.Get<List<MRepository>>(repositoryUrl);
                if (newList != null)
                {
                    repo.AddRange(newList);
                }

                // in here i need to add a new one
                //_settings.SetStringValue("repository_list", );
                localSettings.Values["repository_list"] = JsonSerializer.Serialize(repo);

                LoadRepository();

            }
            else
            {
                await _dialog.MessageDialogAsync("Warning", "You did not add a new repo url correctly..", "Close");
            }
        }


        [RelayCommand]
        public async void LoadRepository()
        {
            try
            {
                SelectedRepo = null;
                //SelectedPluginRepo = null;
                PluginRespositoryList.Clear();
                //var lister = _settings.ReadStringValue("repository_list", "");
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                var lister = localSettings.Values["repository_list"] as string;
                var repo = new List<MRepository>();

                if (!String.IsNullOrEmpty(lister))
                {
                    repo = JsonSerializer.Deserialize<List<MRepository>>(lister);
                }

                if (RespositoryList.Count > 0)
                    RespositoryList.Clear();

                var DefaultRepo = new MRepository
                {
                    description = "MPlayer Default Repo",
                    name = "MPlayer.cz Repo",
                    url = "https://mplayer.cz/repo.json"
                };
                RespositoryList.Add(DefaultRepo);

                foreach (var item in repo)
                {
                    RespositoryList.Add(item);
                }

                //Log.Logger(LogType.debug, nameof(LoadPlugingRepository), $"STEP4 - before repo SUCCESS | Count: {repo.Count}");

            }
            catch (Exception ex)
            {
                //Log.Logger(LogType.error, nameof(LoadRepository), ex);
                Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
                await _dialog.MessageDialogAsync("Error!", ex.Message, "OK");
            }
        }


        [RelayCommand]
        public async void LoadPlugingRepository(MRepository repo)
        {
            try
            {               
                SelectedRepo = repo;
                var pRepository = await _httpService.Get<List<MPluginRepository>>(repo.url!);

                if (pRepository != null)
                {             
                    if (PluginRespositoryList.Count > 0)
                    {
                        PluginRespositoryList.Clear();
                        await Task.Delay(20);
                    }

                    Log.Logger(LogType.debug, nameof(LoadPlugingRepository), $"COUNTER: " + pRepository.Count);

                    foreach (MPluginRepository item in pRepository)
                    {
                        try
                        {
                            PluginRespositoryList.Add(item);
                            Log.Logger(LogType.debug, nameof(LoadPlugingRepository), $"LOOP pRepository\n" + JsonSerializer.Serialize(item));
                        }
                        catch (Exception ex)
                        {
                            Log.Logger(LogType.error, nameof(LoadPlugingRepository), $"LOOP ERRR pRepository\n" + JsonSerializer.Serialize(item));
                        }

                    }

                }
                Log.Logger(LogType.debug, nameof(LoadPlugingRepository), $"END pRepository\n" + JsonSerializer.Serialize(PluginRespositoryList));
            }
            catch (Exception ex)
            {
                //Log.Logger(LogType.error, nameof(LoadPlugingRepository), ex);
                Debug.WriteLine($"Loading Plugin Repository: {ex.Message}");
                await _dialog.MessageDialogAsync("Loading Plugin Repository - Error!", ex.Message, "OK");
            }

        }

        public ObservableCollection<MRepository> RespositoryList { get; set; } = new();

        [ObservableProperty]
        ObservableCollection<MPluginRepository> pluginRespositoryList = new();

        [ObservableProperty]
        MRepository selectedRepo;
    }
}
