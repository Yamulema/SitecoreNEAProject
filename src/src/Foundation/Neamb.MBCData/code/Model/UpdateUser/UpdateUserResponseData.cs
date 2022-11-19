using Neambc.Neamb.Foundation.MBCData.Model.Login;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Model.UpdateUser
{
    public class UpdateUserResponseData
    {
        public string newMdsId { get; set; }
        public int registrationCount { get; set; }
        public List<LoginRegistration> registrations { get; set; }
        public bool updated { get; set; }
    }
}