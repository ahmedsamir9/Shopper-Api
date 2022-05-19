using System.Security.Claims;

namespace ShopperAPi.Helpers
{
    public static class ClaimsExtension
    {
        public static Tuple<string, string> getCurrentUserIdAndEmail(this IEnumerable<Claim> cliams) {
            var email = cliams.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var id = cliams.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return new Tuple<string, string>(id.Value, email.Value);
            
        }
    }
}
