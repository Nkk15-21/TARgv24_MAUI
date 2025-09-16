using Microsoft.Maui.Layouts;

namespace TARgv24;

public partial class DateTimePage : ContentPage
{
	Label mis_on_valitud;
	DatePicker datePicker;
	TimePicker timePicker;
	AbsoluteLayout al;
    public DateTimePage()
	{
		mis_on_valitud = new Label 
		{ 
			Text = "Siin kuvatakse valitud kuupäev või kellaaeg", 
			FontSize = 20, 
			TextColor = Colors.White,
			FontFamily="Lovin Kites 400"
		};
		datePicker = new DatePicker
		{
			FontSize = 20,
			BackgroundColor = Color.FromRgb(200, 200, 100),
			TextColor = Colors.Black,
			FontFamily = "Lovin Kites 400",
			MinimumDate = DateTime.Now.AddDays(-7),//new DateTime(1900, 1, 1),
			MaximumDate = new DateTime(2100, 12, 31),
			Date = DateTime.Now,
			Format="D"
		};
        datePicker.DateSelected += Kuupäeva_valimine;
		al = new AbsoluteLayout { Children = { mis_on_valitud, datePicker } };
		AbsoluteLayout.SetLayoutBounds(mis_on_valitud, new Rect(0.5, 0.2, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
		AbsoluteLayout.SetLayoutFlags(mis_on_valitud, AbsoluteLayoutFlags.All);
		AbsoluteLayout.SetLayoutBounds(datePicker, new Rect(0.5, 0.5, 0.9, 0.2));
		AbsoluteLayout.SetLayoutFlags(datePicker, AbsoluteLayoutFlags.All);
		Content = al;
    }

    private void Kuupäeva_valimine(object? sender, DateChangedEventArgs e)
    {
        mis_on_valitud.Text = $"Valisite kuupäeva: {e.NewDate:D}";
    }
}