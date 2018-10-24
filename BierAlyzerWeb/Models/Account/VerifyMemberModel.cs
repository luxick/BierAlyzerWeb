namespace BierAlyzerWeb.Models.Account
{
    public class VerifyMemberModel
    {
        public string UserName { get; set; }

        public string Chapter { get; set; }

        public string HexId { get; set; }

        public string MessageToAdmin { get; set; }
    }
}
