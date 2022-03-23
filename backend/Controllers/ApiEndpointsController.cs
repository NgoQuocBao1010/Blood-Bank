using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiEndpointsController : ControllerBase
    {
        [HttpGet]
        public List<Dictionary<string, string>> Get(string id)
        {
            var endpoints = new List<Dictionary<string, string>>();
            var endpointsBlood = new Dictionary<string, string>()
            {
                {"GET api/Blood", "Return All of Blood"},
                {"GET api/Blood/${id}", "Return a Blood by ID"},
                {"POST api/Blood", "Create a new Blood"},
                {"PUT api/Blood/${id}", "Update 1 Blood by ID"},
                {"DELETE api/Blood/${id}", "Delete 1 Blood by ID"},
                {"GET api/Blood/type/${name}", "Return a Blood by its name"}
            };
            var endpointsDonor = new Dictionary<string, string>()
            {
                {"GET api/Donor", "Return All of Donor"},
                {"GET api/Donor/{$id}", "Return a Donor by ID"},
                {"POST api/Donor", "Create a new Donor"},
                {"PUT api/Donor/${id}", "Update 1 Donor by ID"},
                {"DELETE api/Donor/${id}", "Delete 1 Donor by ID"}
            };
            var endpointsDonorTransaction = new Dictionary<string, string>()
            {
                {"GET api/DonorTransaction", "Return All of Transaction"},
                {"GET api/DonorTransaction/{$id}", "Return a DonorTransaction by ID"},
                {"POST api/DonorTransaction", "Create a new Transaction"},
                {"PUT api/DonorTransaction/${id}", "Update 1 Transaction by ID"},
                {"DELETE api/DonorTransaction/${id}", "Delete 1 Transaction by ID"}
            };
            var endpointsEvent = new Dictionary<string, string>()
            {
                {"GET api/Event", "Return All of Event"},
                {"GET api/Event/{$id}", "Return  an Event by ID"},
                {"POST api/Event", "Create a new Event"},
                {"PUT api/Event/${id}", "Update 1 Event by ID"},
                {"DELETE api/Event/${id}", "Delete 1 Event by ID"}
            };
            var endpointsEventSubmission = new Dictionary<string, string>()
            {
                {"GET api/EventSubmission", "Return All of EventSubmission"},
                {"GET api/EventSubmission/{$id}", "Return an EventSubmission by ID"},
                {"POST api/EventSubmission", "Create a new EventSubmission"},
                {"PUT api/EventSubmission/${id}", "Update 1 EventSubmission by ID"},
                {"DELETE api/EventSubmission/${id}", "Delete 1 EventSubmission by ID"},
            };
            var endpointsHospital = new Dictionary<string, string>()
            {
                {"GET api/Hospital", "Return All of Hospital"},
                {"GET api/Hospital/{$id}", "Return a Hospital by ID"},
                {"POST api/Hospital", "Create a new Hospital"},
                {"PUT api/Hospital/${id}", "Update 1 Hospital by ID"},
                {"DELETE api/Hospital/${id}", "Delete 1 Hospital by ID"}
            };
            var endpointsUser = new Dictionary<string, string>()
            {
                {"GET api/User", "Return All of User"},
                {"GET api/User/{$id}", "Return an User by ID"},
                {"POST api/User", "Create a new User"},
                {"PUT api/User/${id}", "Update 1 User by ID"},
                {"DELETE api/User/${id}", "Delete 1 User by ID"}
            };
            var endpointsRequest = new Dictionary<string, string>() {
                {"GET api/Request", "Return All of Request"},
                {"GET api/Request/{$id}", "Return a Request by ID"},
                {"POST api/Request", "Create a new Request"},
                {"PUT api/Request/${id}", "Update 1 Request by ID"},
                {"DELETE api/Request/${id}", "Delete 1 Request by ID"}
            };
            endpoints.Add(endpointsBlood);
            endpoints.Add(endpointsDonor);
            endpoints.Add(endpointsDonorTransaction);
            endpoints.Add(endpointsHospital);
            endpoints.Add(endpointsEvent);
            endpoints.Add(endpointsEventSubmission);
            endpoints.Add(endpointsUser);
            endpoints.Add(endpointsRequest);
            
            return endpoints;
        }
    }
}