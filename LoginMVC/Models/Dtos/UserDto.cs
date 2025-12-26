using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginMVC.Models.Dtos
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public enum role
        {
            Trader,
            Supervisor,
            Manager,
            Auditor,
            SystemAdmin
        }
        public bool IsActive { get; set; }
        public string ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public DateTime LastLoginAt { get; set; }

    }
}