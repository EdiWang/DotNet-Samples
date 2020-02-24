using System;

namespace CoronavirusReportWeb.Models
{
    public class InfectionData
    {
        public DateTime Date { get; set; }
        public int Confirm { get; set; }
        public int Suspect { get; set; }
        public int Dead { get; set; }
        public int Heal { get; set; }
    }
}