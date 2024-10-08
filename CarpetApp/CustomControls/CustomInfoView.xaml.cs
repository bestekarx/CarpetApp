using Microsoft.Maui.Controls;

namespace CarpetApp.CustomControls;

public partial class CustomInfoView
{
    private static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomInfoView) );
    public string Title
    {
        get => GetValue(TitleProperty) as string;
        init => SetValue(TitleProperty, value);
    }
    
    private static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(CustomInfoView) );
    public string ImageSource
    {
        get => GetValue(ImageSourceProperty) as string;
        init => SetValue(ImageSourceProperty, value);
    }
    
    public CustomInfoView()
    {
        InitializeComponent();
    }
}