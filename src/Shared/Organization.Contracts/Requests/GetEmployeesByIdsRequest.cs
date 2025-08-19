﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Requests
{
    public record GetEmployeesByIdsRequest(List<Guid> EmployeeIds);
}
