using Dapper;
using ENSEK.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ENSEK.Services
{
    public static class DBConnection
    {
        public static readonly string conString = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

        public static List<MeterReading> GetMeterReadingsData()
        {
            try
            {
                List<MeterReading> obList = new List<MeterReading>();
                using (SqlConnection con = new SqlConnection(conString))
                {
                    obList = con.Query<MeterReading>("SELECT * FROM MeterReadings").ToList();
                }
                return obList;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int InsertReadindData(int AccountID,string DateTime,string ReadingValue, ref int status)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@AccountId", AccountID);
                p.Add("@MeterReadingDateTime", DateTime);
                p.Add("@MeterReadValue", ReadingValue);
                p.Add("@RefID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (SqlConnection con = new SqlConnection(conString))
                {
                    status = con.Execute("Sp_AddMeterReadings",
                     p,
                     commandType: CommandType.StoredProcedure);
                }
                status = p.Get<Int32>("@RefID");
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static int CheckAccountIDExists(int AccountID, ref int status)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@AccountId", AccountID);
                p.Add("@RefID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (SqlConnection con = new SqlConnection(conString))
                {
                    status = con.Execute("CheckTest_AccountNoExist",
                     p,
                     commandType: CommandType.StoredProcedure);
                }
                status = p.Get<Int32>("@RefID");
                return status;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}