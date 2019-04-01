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

        public static DBOperations GetInstance()
        {
            if (instance == null)
            {
                instance = new DBOperations();
            }

            return instance;
        }
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

                /*
                    patid uuid,
                    patname character varying,
                    phonenum integer,
                    email character varying,
                    address character varying,
                    insuranceid uuid,
                    insuranceplan character varying,
                    physicianid uuid
                 */
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
                if((connection != null) && (connection.State != ConnectionState.Open))
                {
                    connection.Close();
                }
            }
            return false;
        }
    }
}
