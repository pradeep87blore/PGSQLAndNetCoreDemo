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

        /*
         *  Facts are tests which are always true. They test invariant conditions.
            Theories are tests which are only true for a particular set of data.
         */
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

        [Fact]
        public void FetchPatientInfoTest()
        {
            DBOperations dbOperations = DBOperations.GetInstance();
            int initialPatientCount = DBOperations.GetInstance().GetPatientCount();
            Guid newPatientId = Guid.Empty;

            Assert.True(AddSinglePatient(out newPatientId));

            PatientInfo patInfo;
            InsuranceInfo insuranceInfo;
            PhysicianDetails physDetails;

            Assert.True(DBOperations.GetInstance().GetPatient(
                newPatientId, out patInfo, out insuranceInfo, out physDetails));

            Console.WriteLine(patInfo.ToString());

            Assert.Equal(newPatientId, patInfo.m_patientId);
            Assert.Equal(newPatientId, insuranceInfo.m_patientId);
        }



        private bool AddNewPatients(int numOfPatientsToAdd)
        {
            

            for (int iIndex = 0; iIndex < numOfPatientsToAdd; iIndex++)
            {
                Guid newPatientId = Guid.Empty;
                if (!AddSinglePatient(out newPatientId)) // newPatientId not used here
                {
                    return false;
                }
            }

            return true;
        }

        private bool AddSinglePatient(out Guid newPatientId)
        {
            var rand = new Random();

            newPatientId = Guid.Empty;

            string name = commonNames[rand.Next(commonNames.Length - 1)];
            // Create a random patient info
            PatientInfo patIfno = new PatientInfo(name, rand.Next(11111, 99999),
                name + "@" + "abcd.com", "aaaa", Guid.Empty);

            
            InsuranceInfo insuranceInfo = new InsuranceInfo(patIfno.m_patientId, "BasicMedicalPlan");
            patIfno.m_insuranceId = insuranceInfo.m_insuranceId;
            PhysicianDetails physicianDetails =
                new PhysicianDetails(commonNames[rand.Next(commonNames.Length - 1)], "General Medicine");

            if (!DBOperations.GetInstance().CreateNewPatient(patIfno, insuranceInfo, physicianDetails))
                return false;

            newPatientId = patIfno.m_patientId;

            return true;
        }
    }
}
