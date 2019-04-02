using System;
using PGSqlAccess;
using Xunit;

namespace PatientDBTester
{
    public class PatientDBTester
    {
        private static string[] commonNames =
        {
            "Oliver", "Olivia", "Liam", "Emma",
            "Charlotte", "Gabriel", "Louise", "Hugo",
            "Sofia", "Alexander"
        };

        [Fact]
        public void AddPatientsTest()
        {
            int patientCount = 25;
            DBOperations dbOperations = DBOperations.GetInstance();
            Assert.True(AddNewPatients(patientCount));
        }

        [Fact]
        public void CheckForNewPatientsTest()
        {
            int newPatientCount = 15;
            DBOperations dbOperations = DBOperations.GetInstance();
            int initialPatientCount = DBOperations.GetInstance().GetPatientCount();
            Assert.True(AddNewPatients(newPatientCount));
            int updatedPatientCount = DBOperations.GetInstance().GetPatientCount();

            Assert.True((updatedPatientCount - initialPatientCount) == newPatientCount);
        }

        [Fact]
        public void DeletePatientsTest()
        {
            int newPatientCount = 15;
            DBOperations dbOperations = DBOperations.GetInstance();
            int initialPatientCount = DBOperations.GetInstance().GetPatientCount();
            Assert.True(AddNewPatients(newPatientCount));
            int updatedPatientCount = DBOperations.GetInstance().GetPatientCount();
            Assert.True((updatedPatientCount - initialPatientCount) == newPatientCount);

            Assert.True(DBOperations.GetInstance().DeleteAllPatients());

            Assert.Equal(0, DBOperations.GetInstance().GetPatientCount());
        }


        private bool AddNewPatients(int numOfPatientsToAdd)
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

                if (!DBOperations.GetInstance().CreateNewPatient(patIfno, insuranceInfo, physicianDetails))
                    return false;
            }

            return true;
        }

    }
}
