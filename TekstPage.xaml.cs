namespace TARgv24;

public partial class TekstPage : ContentPage
{
	Label lblTekst;
	Editor editorTekst;
	HorizontalStackLayout hsl;
    public TekstPage()
	{
		lblTekst = new Label 
		{ 
			Text = "Tekst: ", 
			FontSize = 20, 
			TextColor = Colors.White,
			FontFamily="Lovin Kites 400"
		};
		editorTekst = new Editor
		{
			FontSize = 20,
			BackgroundColor = Color.FromRgb(200, 200, 100),
			TextColor = Colors.Black,
			FontFamily = "Lovin Kites 400",
			AutoSize = EditorAutoSizeOption.TextChanges,
			Placeholder = "Siia tuleb tekst",
			PlaceholderColor = Colors.Gray,
			FontAttributes = FontAttributes.Italic
		};
        editorTekst.TextChanged += EditorTekst_TextChanged;
        hsl = new HorizontalStackLayout { 
			BackgroundColor = Color.FromRgb(120, 30, 50),
			Children = { lblTekst, editorTekst },
			HorizontalOptions = LayoutOptions.Center
        };
		Content = hsl;
    }
    private void EditorTekst_TextChanged(object? sender, TextChangedEventArgs e)
    {
        lblTekst.Text=editorTekst.Text;
    }
}