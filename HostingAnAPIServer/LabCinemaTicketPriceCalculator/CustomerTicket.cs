using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCinemaTicketPriceCalculator
{
    internal class CustomerTicket
    {
        public string FilmID { get; set; }
        public string Title { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return AdultCount * adultPrice + ChildCount * childPrice;
            }
        }
        private decimal adultPrice = 5.50M;
        private decimal childPrice = 2.50M;
    }
}
