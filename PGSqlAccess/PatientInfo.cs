using System;
using System.Collections.Generic;
using System.Text;

namespace PGSqlAccess
{
    public class PatientInfo
    {
        public Guid m_patientId { get; }
        public string m_patientName;
        public int m_phoneNum;
        public string m_emailId;
        public string m_address;
        public Guid m_insuranceId;


        public PatientInfo(string patientName, int phoneNum, string emailId, 
            string address, Guid insuranceId)
        {
            m_patientId = Guid.NewGuid();

            m_patientName = patientName;
            m_phoneNum = phoneNum;
            m_emailId = emailId;
            m_address = address;
            m_insuranceId = insuranceId;
        }

        public override string ToString()
        {
            string printString =
                string.Format(
                    "Patient Name: {0}, Patient Id: {1}, Phone num: {2}, Email: {3}, Address = {4}, InsuranceId: = {5}",
                    m_patientName, m_patientId, m_phoneNum, m_emailId, m_address, m_insuranceId);

            return printString;
        }
    }
}
