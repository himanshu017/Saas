using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace AdminPanel.Services.Helpers
{
    public class ImportHelper
    {
        public readonly IConfiguration _config;

        public ImportHelper(IConfiguration config)
        {
            _config = config;
        }
            public bool? ValidateBooleanValue(string inputval)
        {
            try
            {
                if (inputval == "NULL" || string.IsNullOrWhiteSpace(inputval))
                    return null;
                else if (inputval == "0" || inputval == "N")
                    return false;

                else if (inputval == "1" || inputval == "Y")
                    return true;

                else
                    return null;
            }
            catch (Exception)
            {

                return null;
            }


        }
        public double? ValidateDoubleValue(string inputval)
        {
            try
            {
                if (inputval == "NULL" || string.IsNullOrWhiteSpace(inputval))
                    return null;

                else
                    return Convert.ToDouble(inputval);
            }
            catch (Exception)
            {

                return null;
            }


        }
        public string? ValidatestringValue(string inputval)
        {
            try
            {
                if (inputval == "NULL" || string.IsNullOrWhiteSpace(inputval))
                    return null;

                else
                    return Convert.ToString(inputval);
            }
            catch (Exception)
            {

                return null;
            }


        }
        public Int32? ValidateInt32Value(string inputval)
        {
            try
            {
                if (inputval == "NULL" || string.IsNullOrWhiteSpace(inputval))
                    return null;

                else
                    return Convert.ToInt32(inputval);
            }
            catch (Exception)
            {

                return null;
            }



        }
        public Int64? ValidateInt64Value(string inputval)
        {
            try
            {
                if (inputval == "NULL" || string.IsNullOrWhiteSpace(inputval))
                    return null;

                else
                    return Convert.ToInt64(inputval);
            }
            catch (Exception)
            {

                return null;
            }



        }

        public DateTime? ValidateDateTimeValue(string inputval)
        {


            if (inputval == "NULL" || inputval == null || string.IsNullOrEmpty(inputval))
                return null;

            string[] formats = new string[13] { "M-d-yyyy", "dd-MM-yyyy", "MM-dd-yyyy", "M.d.yyyy", "dd.MM.yyyy", "MM.dd.yyyy", "M/d/yyyy", "M/d/yy", "MM/dd/yy", "MM/dd/yyyy", "yy/MM/dd", "yyyy-MM-dd", "dd-MMM-yy" };

            try
            {
                DateTime result = DateTime.ParseExact(inputval, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                return result;
            }
            catch (Exception ex)
            {
                try
                {
                    return Convert.ToDateTime(inputval);
                }
                catch (Exception exp)
                {
                    return null;
                }
                // your error handling code here
                //return null;
            }
        }

        public void SqlTruncateTable(string tableName, long companyID)
        {

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("OrderflowConnString")))
                {
                    dbConnection.Open();
                    SqlCommand cmd = new SqlCommand($"Delete from {tableName} where companyid={companyID};", dbConnection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
