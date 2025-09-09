namespace TARgv24;

public partial class FigurePage : ContentPage
{
    BoxView boxView;
    Random random = new Random();
    HorizontalStackLayout hsl;

    public FigurePage()
    {
        // Generate random RGB color
        int red = random.Next(0, 256);
        int green = random.Next(0, 256);
        int blue = random.Next(0, 256);

        // Get screen size adjusted for density
        var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

        boxView = new BoxView
        {
            Color = Color.FromRgb(red, green, blue),
            WidthRequest = screenWidth / 2,
            HeightRequest = screenHeight / 4,
            CornerRadius = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromRgba(0, 0, 0, 0) // Transparent
        };

        // Add TapGestureRecognizer to the BoxView
        TapGestureRecognizer tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += KLik_boksi_peal;
        boxView.GestureRecognizers.Add(tapGesture);

        // Add the BoxView to a HorizontalStackLayout
        hsl = new HorizontalStackLayout
        {
            Children = { boxView }
        };

        // Set the content of the page
        Content = hsl;
    }

    private void KLik_boksi_peal(object? sender, TappedEventArgs e)
    {
        // Get screen size adjusted for density
        var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

        // Change color and size randomly
        boxView.Color = Color.FromRgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
        boxView.WidthRequest = random.Next(50, (int)(screenWidth / 1.5));
        boxView.HeightRequest = random.Next(50, (int)(screenHeight / 1.5));
        boxView.CornerRadius = random.Next(0, 51);
    }
}
