namespace Wallone.Core.Interfaces
{
    public interface IUserShort : IUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
    }
}