using ENSEK.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ENSEK.Services
{
    public static class MeterReadingService
    {

        public static int InsertMeterReadings(int AccountID,string DateTime,string ReadingValue,ref int status)
        {
            ReadingValue = ReadingValue.PadLeft(5, '0');
            return DBConnection.InsertReadindData(AccountID, DateTime, ReadingValue,ref status);
        }
        public static List<MeterReading> GetMeterReadingsData()
        {
            return DBConnection.GetMeterReadingsData();
        }
        public static bool checkAccoutIdExists(int AccountID)
        {
            bool IsExist = false;
            int status = 0;
            DBConnection.CheckAccountIDExists(AccountID, ref status);
            if (status == 1)
            {
                IsExist = true;
            }
            return IsExist;
        }
        public static bool checkReadingValueisNumber(string MeterValue)
        {
            double number;
            bool isNumber = false;
            if (double.TryParse(MeterValue, out number))
            {
                if (number > 0)
                {
                    isNumber = true;
                }
                else
                {
                    isNumber = false;
                }
            }
            return isNumber;

        }
        public static MeterReadingResponce MeterReadingProcess(DataTable dt)
        {
            List<int> lstNotExistedAccountIds = new List<int>();
            List<int> lstSucessAccountIds = new List<int>();
            List<int> lstFailedAccountIds = new List<int>();
            MeterReadingResponce objResponce = new MeterReadingResponce();
            foreach (DataRow row in dt.Rows)
            {
                int ID = (int)row["AccountId"];
                string DateTime = row["MeterReadingDateTime"].ToString();
                string MeterReading = row["MeterReadValue"].ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""); ;
                bool isNumber = checkReadingValueisNumber(MeterReading);

                if (isNumber)
                {
                    //condition to check the Account Id is Exist in TestAccount Table if Exist Insert in Table Else NO
                    bool isExist = checkAccoutIdExists(ID);
                    if (isExist)
                    {
                        int status = 0;
                        InsertMeterReadings(ID, DateTime, MeterReading, ref status);
                        if (status == 1)
                        {
                            lstSucessAccountIds.Add(ID);
                        }
                        else
                        {
                            lstFailedAccountIds.Add(ID);
                        }
                    }
                    else
                    {
                        lstNotExistedAccountIds.Add(ID);
                    }
                }
                else
                {
                    lstFailedAccountIds.Add(ID);
                }
            }
            objResponce.SucessAccountIDs = lstSucessAccountIds;
            objResponce.NotMatchedAccountIDs = lstNotExistedAccountIds;
            objResponce.FailedAccountIDs = lstFailedAccountIds;
            objResponce.MeterReadings = GetMeterReadingsData();
            return objResponce;
        }
        public static DataTable CreateDataTableForCSVFile(string filePath)
        {
            //Create a DataTable.
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("AccountId", typeof(int)),
                        new DataColumn("MeterReadingDateTime", typeof(string)),
                        new DataColumn("MeterReadValue", typeof(string))});

            //Read the contents of CSV file.
            string csvData = File.ReadAllText(filePath);
            bool firstRow = true;
            //Execute a loop over the rows.
            foreach (string row in csvData.Split('\n'))
            {
                if (firstRow)
                {
                    firstRow = false;
                }
                else if (!string.IsNullOrEmpty(row))
                {
                    dt.Rows.Add();
                    int i = 0;

                    //Execute a loop over the columns.
                    foreach (string cell in row.Split(','))
                    {
                        if (i != 3)
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell;
                            i++;
                        }
                    }
                }
            }
            return dt;
        }
    }
}