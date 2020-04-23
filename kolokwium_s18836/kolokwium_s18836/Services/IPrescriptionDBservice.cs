using kolokwium_s18836.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kolokwium_s18836.Services
{
    public interface IPrescriptionDBservice
    {
        void add_prescription(prescriptionsresponse prescription);

        List<prescriptionsresponse> get_response();

        void clear();

    }
}
