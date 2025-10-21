namespace TARgv24;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // (не обязательно, но можно)
        Routing.RegisterRoute("start", typeof(StartPage));
       // Routing.RegisterRoute("main", typeof(MainPage));
        Routing.RegisterRoute("valgusfoor", typeof(ValgusfoorPage));
        Routing.RegisterRoute("timer", typeof(TimerPage));
        Routing.RegisterRoute("lumememm", typeof(LumememmPage));
        Routing.RegisterRoute("figure", typeof(FigurePage));
        Routing.RegisterRoute("tekst", typeof(TekstPage));
        Routing.RegisterRoute("datetime", typeof(DateTimePage));
    }
}
