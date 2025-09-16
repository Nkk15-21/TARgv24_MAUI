using Microsoft.Maui.Controls;  // For controls like Label, Picker, DatePicker, TimePicker, etc.
using Microsoft.Maui.Graphics;  // For handling colors and graphics
using Microsoft.Maui.Layouts;  // For AbsoluteLayout and other layout elements


namespace TARgv24;

public partial class DateTimePage : ContentPage
{
	Label mis_on_valitud;
	DatePicker datePicker;
	TimePicker timePicker;
	Picker picker;
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
		timePicker = new TimePicker
		{
			BackgroundColor = Color.FromRgb(200,200,100),
			TextColor = Colors.Black,
			FontFamily = "Lovin Kites 400",
			Time = new TimeSpan(12,0,0),
			Format="T"

		};
		timePicker.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
			{
				mis_on_valitud.Text = $"Valisite kellaaja: {timePicker.Time}";
			}
		};

		picker = new Picker
		{
			Title = "Vali üks"
		};
		picker.Items.Add("Kuus");
		picker.SelectedIndexChanged += (s, e) =>
		{
			if (picker.SelectedIndex != -1)
			{
				mis_on_valitud.Text = $"Valisite: {picker} "
			}
		};


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