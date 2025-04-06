namespace CarpetApp.CustomControls;

public partial class CustomInfoView
{
  private static readonly BindableProperty TitleProperty =
    BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomInfoView));

  private static readonly BindableProperty ImageSourceProperty =
    BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(CustomInfoView));

  public CustomInfoView()
  {
    InitializeComponent();
  }

  public string Title
  {
    get => GetValue(TitleProperty) as string;
    init => SetValue(TitleProperty, value);
  }

  public string ImageSource
  {
    get => GetValue(ImageSourceProperty) as string;
    init => SetValue(ImageSourceProperty, value);
  }
}