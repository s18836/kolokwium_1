using kolokwium_s18836.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kolokwium_s18836.Services
{
    public class PrescriptionDBservice : IPrescriptionDBservice
    {
        List<prescriptionsresponse> responses = new List<prescriptionsresponse>();

        public void add_prescription(prescriptionsresponse prescription)
        {
            responses.Add(prescription);           
        }

        public List<prescriptionsresponse> get_response()
        {
            return responses;
        }


        void IPrescriptionDBservice.clear()
        {
            responses.Clear();
        }
    }
}
