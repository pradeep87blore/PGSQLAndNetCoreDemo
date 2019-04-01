using System;
using System.Collections.Generic;
using System.Text;

namespace PGSqlAccess
{
    public class InsuranceInfo
    {
        public Guid m_insuranceId { get; }
        public Guid m_patientId;
        public string m_insurancePlan;

        public InsuranceInfo(Guid patientId, string insurancePlan)
        {
            m_insuranceId = Guid.NewGuid();
            m_patientId = patientId;
            m_insurancePlan = insurancePlan;
        }
    }
}
