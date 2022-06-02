namespace Wallone.Core.Interfaces
{
    public interface IUserShort : IUser
    {
        public string id { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
    }
}