
namespace Neambc.Neamb.Foundation.MBCData.Model.Login
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UnionId { get; set; }
        public string CellCode { get; set; }
        public string MatchRoutineIdentifier { get; set; }
    }
}