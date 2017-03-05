namespace OrangeBricks.Web.Controllers.Home.ViewModels
{
    public class BuyerHomeViewModel
    {
        public int OffersCount { get; set; }
        public int AcceptedOffersCount { get; set; }
        public int RejectedOffersCount { get; set; }
        public int AppointmentsCount { get; set; }
        public int AcceptedAppointments { get; set; }
        public int DeclinedAppointments { get; set; }
    }
}