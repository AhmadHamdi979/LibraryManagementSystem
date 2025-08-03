﻿using Library.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Auth.Commands
{
    public record RegisterUserCommand(RegisterRequest Request) : IRequest<string>; 
    
    
}
