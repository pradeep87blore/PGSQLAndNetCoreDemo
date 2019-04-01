using System;
using System.Collections.Generic;
using System.Text;

namespace PGSqlAccess
{

    public class DBTester
    {
        private static string[] commonNames =
        {
            "Oliver", "Olivia", "Liam", "Emma",
            "Charlotte", "Gabriel", "Louise", "Hugo",
            "Sofia", "Alexander"
        };



        public DBTester()
        {
            
        }
        public void AddNewPatients(int numOfPatientsToAdd)
        {
            var rand = new Random();

            for (int iIndex = 0; iIndex < numOfPatientsToAdd; iIndex++)
            {
                string name = commonNames[rand.Next(commonNames.Length - 1)];
                PatientInfo patIfno = new PatientInfo(name, rand.Next(11111, 99999),
                    name + "@" + "abcd.com", "aaaa", Guid.Empty);
                InsuranceInfo insuranceInfo = new InsuranceInfo(patIfno.m_patientId, "BasicMedicalPlan");
                patIfno.m_insuranceId = insuranceInfo.m_insuranceId;
                PhysicianDetails physicianDetails = 
                    new PhysicianDetails(commonNames[rand.Next(commonNames.Length - 1)], "General Medicine");

                DBOperations.GetInstance().CreateNewPatient(patIfno, insuranceInfo, physicianDetails);
            }
        }

    }
}
