﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface;

public interface IJwtService
{
    string ValidateToken(string token);

}
