using System;
using Wallone.Core.Interfaces;

namespace Wallone.Core.Models
{
    public class UserShort : IUserShort
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
    }
}