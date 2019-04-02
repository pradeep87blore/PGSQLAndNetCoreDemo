using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PGSqlAccess
{

    public class DBOperations
    {
        private static DBOperations instance = null;

        DBOperations() // Private constructor, to enforce the use of the singleton instance
        {

        }

        public static DBOperations GetInstance()
        {
            if (instance == null)
            {
                instance = new DBOperations();
            }

            return instance;
        }

        // TODO: Physician name not used and stored anywhere. Create a separate table for this
        public bool CreateNewPatient(PatientInfo patInfo,
           InsuranceInfo insuranceInfo,
            PhysicianDetails physDetails)
        {
            NpgsqlConnection connection = null;

            try
            {
                PGSqlAccessHelper pgaccess = PGSqlAccessHelper.GetInstance();

                if (!pgaccess.ConnectToDB(out connection))
                {
                    throw new Exception("Failed to connect to the PGSQL DB");
                }

                string sqlCmd =
                    String.Format(@"CALL spcreatepatient('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}')",
                        patInfo.m_patientId, patInfo.m_patientName, patInfo.m_phoneNum,
                        patInfo.m_emailId, patInfo.m_address, insuranceInfo.m_insuranceId,
                        insuranceInfo.m_insurancePlan, physDetails.m_PhysicianId);

                using (var cmd = new NpgsqlCommand(sqlCmd, connection))
                {
                    cmd.ExecuteNonQuery();
                }


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if ((connection != null) && (connection.State != ConnectionState.Open))
                {
                    connection.Close();
                }
            }
            return false;
        }

        // Delete all the created patients
        public bool DeleteAllPatients()
        {
            NpgsqlConnection connection = null;

            try
            {
                PGSqlAccessHelper pgaccess = PGSqlAccessHelper.GetInstance();

                if (!pgaccess.ConnectToDB(out connection))
                {
                    throw new Exception("Failed to connect to the PGSQL DB");
                }

                string sqlCmd = "call spdeleteallpatients()";
                using (var cmd = new NpgsqlCommand(sqlCmd, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Nothing to do here
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if ((connection != null) && (connection.State != ConnectionState.Open))
                {
                    connection.Close();
                }
            }

            return false;
        }

        // Return the count of the existing patients
        public int GetPatientCount()
        {
            NpgsqlConnection connection = null;

            int patCount = 0;
            try
            {
                PGSqlAccessHelper pgaccess = PGSqlAccessHelper.GetInstance();

                if (!pgaccess.ConnectToDB(out connection))
                {
                    throw new Exception("Failed to connect to the PGSQL DB");
                }

                Int64 counter = 0; // TODO: This is of no use now. Need to find out how to discard this
                string sqlCmd = String.Format(@"call sppatientcount('{0}')", counter);
                using (var cmd = new NpgsqlCommand(sqlCmd, connection))
                {
                    //cmd.Parameters.AddWithValue("counter", counter);
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows == false)
                            return 0; // Error in reading the patient count
                        else
                        {
                            var value = reader.GetInt64(0);
                            patCount = Int32.Parse(reader[0].ToString());
                        }
                    }
                }
                return patCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if ((connection != null) && (connection.State != ConnectionState.Open))
                {
                    connection.Close();
                }
            }
            return patCount;
        }

        // Fetch a specific patient's details
        // Using a direct SQL query here
        public bool GetPatient(Guid patientId, out PatientInfo patInfo, out InsuranceInfo insuranceInfo,
            out PhysicianDetails physDetails)
        {
            patInfo = null;
            insuranceInfo = null;
            physDetails = null;

            NpgsqlConnection connection = null;

            int patCount = 0;
            try
            {
                PGSqlAccessHelper pgaccess = PGSqlAccessHelper.GetInstance();

                if (!pgaccess.ConnectToDB(out connection))
                {
                    throw new Exception("Failed to connect to the PGSQL DB");
                }
                
                string sqlCmd = "SELECT \"Name\", public.\"PatientDetails\".\"PatientId\", \"PhoneNum\", \"EmailId\", \"Address\", "+
                                "\"PhysicianId\", \"FirstAdmissionDate\", \"LatestVisitDate\", public.\"InsuranceInfo\".\"InsuranceId\", "+
                                "\"InsurancePlan\"  FROM public.\"PatientDetails\", public.\"PatientInfo\", public.\"InsuranceInfo\" where "+
                                string.Format("\"PatientDetails\".\"PatientId\" = '{0}' and \"PatientInfo\".\"PatientID\" = '{0}' and\"InsuranceInfo\".\"PatientId\" = '{0}';",patientId);
                using (var cmd = new NpgsqlCommand(sqlCmd, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows == false)
                            return false; // Matching patient not found
                        else
                        {
                            /*
                             * SELECT "Name", public."PatientDetails"."PatientId", "PhoneNum", 
		"EmailId", "Address", "PhysicianId", "FirstAdmissionDate", 
		"LatestVisitDate", public."InsuranceInfo"."InsuranceId", "InsurancePlan"
		FROM public."PatientDetails", public."PatientInfo", public."InsuranceInfo"
		where "PatientDetails"."PatientId" = patientid 
		and 
		"PatientInfo"."PatientID" = patientid 
		and
		"InsuranceInfo"."PatientId" = patientid;
                             */
                            var name = reader[0];
                            patInfo = new PatientInfo(reader.GetString(0), reader.GetInt32(2),
                                reader.GetString(3), reader.GetString(4), reader.GetGuid(8), reader.GetGuid(1));
                            insuranceInfo = new InsuranceInfo(patientId, reader.GetString(9));
                            physDetails = new PhysicianDetails("ABCD", "DEFH"); // TODO: Physician details are not stored. Need to add these

                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if ((connection != null) && (connection.State != ConnectionState.Open))
                {
                    connection.Close();
                }
            }

            return false;
        }

        // Get all patients' info
        // Return the count of the retrieved patients
        public int GetAllPatientsInfo(out List<PatientInfo> patInfoList)
        {
            patInfoList = null;
            return 0;
        }

        // Get all insurance' info
        // Return the count of the retrieved patients
        public int GetAllInsuranceInfo(out List<InsuranceInfo> insInfoList)
        {
            insInfoList = null;
            return 0;
        }

        // Get all Physicians' info
        // Return the count of the retrieved patients
        public int GetAllPhysicianInfo(out List<InsuranceInfo> physInfoList)
        {
            physInfoList = null;
            return 0;
        }

        // Edit patient details
        // Enable changing the name, contact details, insurance details and physician name
        public bool UpdatePatientDetails(PatientInfo updatedPatInfo)
        {
            return false;
        }

    }
}
