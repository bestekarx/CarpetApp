using CarpetApp.ViewModels.Base;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace CarpetApp.Services;

public class ContentPageBase : ContentPage
{
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IViewModelBase model && !model.IsInitialized)
        {
            if (!model.IsInitialized)
            {
                model.IsInitialized = true;
                await model.InitializeAsync();
            }

            if (App.Current.Resources.TryGetValue("MainBgColor", out var colorvalue))
            {
                BackgroundColor = (Color)colorvalue;
            }
                
            model.OnViewAppearing();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is IViewModelBase model)
        {
            model.OnViewDisappearing();
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        if (BindingContext is IViewModelBase model)
        {
            model.OnViewNavigatedFrom(args);
        }
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);

        if (BindingContext is IViewModelBase model)
        {
            model.OnViewNavigatingFrom(args);
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is IViewModelBase model)
        {
            model.OnViewNavigatedTo(args);
        }
    }
}

