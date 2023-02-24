using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MPlayer.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy = false;
              
        public bool IsNotBusy => !IsBusy;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading1))]
        bool isLoading1 = false;

        public bool IsNotLoading1 => !IsLoading1;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading2))]
        bool isLoading2 = false;

        public bool IsNotLoading2 => !IsLoading2;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading3))]
        bool isLoading3 = false;

        public bool IsNotLoading3 => !IsLoading3;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading4))]
        bool isLoading4 = false;

        public bool IsNotLoading4 => !IsLoading4;

    }
}
