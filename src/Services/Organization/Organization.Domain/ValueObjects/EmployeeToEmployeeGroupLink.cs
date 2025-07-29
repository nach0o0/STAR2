using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.ValueObjects
{
    public class EmployeeToEmployeeGroupLink
    {
        public Guid EmployeeId { get; private set; }
        public Guid EmployeeGroupId { get; private set; }
        public Guid? HourlyRateId { get; private set; }

        private EmployeeToEmployeeGroupLink() { }

        public EmployeeToEmployeeGroupLink(Guid employeeId, Guid employeeGroupId, Guid? hourlyRateId)
        {
            EmployeeId = employeeId;
            EmployeeGroupId = employeeGroupId;
            HourlyRateId = hourlyRateId;
        }

        public void SetHourlyRate(Guid? hourlyRateId)
        {
            HourlyRateId = hourlyRateId;
        }
    }
}
