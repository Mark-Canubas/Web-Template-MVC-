using LoginMVC.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginMVC.Models.Api
{ 
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Token { get; set; }

        public UserDto User { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime TokenExpiration { get; set; }
    }

}