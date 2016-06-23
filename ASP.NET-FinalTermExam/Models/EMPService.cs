using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ASP.NET_FinalTermExam.Models
{
    public class EMPService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        /// <summary>
        /// 依照條件取得員工資料
        /// </summary>
        /// <returns></returns>
        public List<Models.EMP> GetEMPByCondtioin(Models.EMPSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select HE.[EmployeeID] as 編號,HE.[FirstName]+HE.[LastName] as 姓名,DCT.CodeVal as 職稱,HE.[HireDate] as 任職日期,DCG.CodeVal as 性別,datediff(year,[BirthDate],getdate()) as 年齡
                            from [HR].[Employees] HE join [dbo].[CodeTable] DCT on DCT.[CodeId]=HE.[Title] and DCT.CodeType='TITLE' join [dbo].[CodeTable] DCG on DCG.[CodeId]=HE.[Gender] and DCG.CodeType='GENDER' join [dbo].[CodeTable] DCCI on DCCI.[CodeId]=HE.[City] and DCCI.CodeType='CITY'join [dbo].[CodeTable] DCCO on DCCO.[CodeId]=HE.[Country] and DCCO.CodeType='COUNTRY'
					Where DCCI.[CodeId] = @CITY AND DCCO.[CodeId] = @COUNTRY AND DCG.[CodeId] = @GENDER";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CITY", arg.City == -1 ? -1 : arg.City));
                cmd.Parameters.Add(new SqlParameter("@COUNTRY", arg.Country== null ? string.Empty : arg.Country));
                cmd.Parameters.Add(new SqlParameter("@GENDER", arg.Gender == null ? string.Empty : arg.Gender));
                // cmd.Parameters.Add(new SqlParameter("@EmpId", arg.EmpId == -1 ? -1 : arg.EmpId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt);
        }
        /// <summary>
        /// 刪除員工
        /// </summary>
        public void DeleteEMPById(string EMPId)
        {
            try
            {                
                string sql = "Delete FROM [HR].[Employees] Where [EmployeeID]=@EMPid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@EMPid", EMPId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Models.EMP> MapOrderDataToList(DataTable orderData)
        {
            List<Models.EMP> result = new List<EMP>();
            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new EMP()
                {
                    Id =(int) row["編號"],
                    Name = row["姓名"].ToString(),
                    Title = row["職稱"].ToString(),
                    Hiredate = row["任職日期"] == DBNull.Value ? (DateTime?)null : (DateTime)row["任職日期"],
                    Genter = row["性別"].ToString(),
                    Age = (int)row["年齡"]
                });
            }
            return result;
        }
        
    }
        
}
