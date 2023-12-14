using Identity.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.User.Queries
{
    public class GetUserDetailsByUserNameQuery : IRequest<UserDetailsResponseDto>
    {
        public string Username { get; set; }
    }
}
