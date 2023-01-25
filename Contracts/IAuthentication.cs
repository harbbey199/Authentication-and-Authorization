using Credential_Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthentication
    {
        Task<string> Login(LoginModel user);
        Task<string> Register(RegisterDto registerDto);
    }
}
