using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kolokwium_s18836.DTOs
{
    public class prescriptionsresponse
    {

        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string PatientLastName { get; set; }
        public string DoctorLastName { get; set; }

    }
}
