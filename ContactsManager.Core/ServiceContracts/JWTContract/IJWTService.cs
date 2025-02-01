using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTOs.AuthenticationJWT;

namespace ContactsManager.Core.ServiceContracts.JWTContract
{
    public interface IJWTService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
    }
}

