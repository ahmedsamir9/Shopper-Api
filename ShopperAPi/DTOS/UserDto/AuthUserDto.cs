namespace ShopperAPi.DTOS
{
    public class AuthUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
