using System;
using System.Collections.Generic;
using System.Text;

namespace PGSqlAccess
{
    public class PhysicianDetails
    {
        public Guid m_PhysicianId { get; }
        public string m_physicianName;
        public string m_physicianSpeciality;

        public PhysicianDetails(string physicianName, string physicianSpeciality)
        {
            m_PhysicianId = Guid.NewGuid();
            m_physicianName = physicianName;
            m_physicianSpeciality = physicianSpeciality;
        }
    }
}
