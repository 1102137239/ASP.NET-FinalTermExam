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
        /// 新增訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單編號</returns>
       /* public int InsertOrder(Models.EMP order)
        {
            string sql = @" Insert INTO Sales.Orders
						 (
							CustomerID,EmployeeID,orderdate,requireddate,shippeddate,shipperid,freight,
							shipname,shipaddress,shipcity,shipregion,shippostalcode,shipcountry
						)
						VALUES
						(
							@custid,@empid,@orderdate,@requireddate,@shippeddate,@shipperid,@freight,
							@shipname,@shipaddress,@shipcity,@shipregion,@shippostalcode,@shipcountry
						)
						Select SCOPE_IDENTITY()
						";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@empid", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@requireddate", order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@shippeddate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipperName));
                cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));

                orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return orderId;

        }*/


        /// <summary>
        /// 依照Id 取得訂單資料
        /// </summary>
        /// <returns></returns>
        public Models.EMP GetOrderById(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
					A.OrderId,A.CustomerID,B.Companyname As CustName,
					A.EmployeeID,C.lastname+ C.firstname As EmpName,
					A.Orderdate,A.RequireDdate,A.ShippedDate,
					A.ShipperId,D.companyname As ShipperName,A.Freight,
					A.ShipName,A.ShipAddress,A.ShipCity,A.ShipRegion,A.ShipPostalCode,A.ShipCountry
					From Sales.Orders As A 
					INNER JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					INNER JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					inner JOIN Sales.Shippers As D ON A.shipperid=D.shipperid
					Where  A.OrderId=@OrderId";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt).FirstOrDefault();
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
					Where DCCI.[CodeId] = @CITY";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CITY", arg.City == -1 ? -1 : arg.City));
                //cmd.Parameters.Add(new SqlParameter("@Orderdate", arg.OrderDate == null ? string.Empty : arg.OrderDate));
               // cmd.Parameters.Add(new SqlParameter("@EmpId", arg.EmpId == -1 ? -1 : arg.EmpId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt);
        }
      

        /// <summary>
        /// 修改訂單
        /// </summary>
        /// <param name="order"></param>
      /*  public void UpdateOrder(Models.EMP order,int id)
        {
            string sql = @"Update 
							Sales.Orders SET 
							CustomerID=@custid,EmployeeID=@empid,
							orderdate=@orderdate,requireddate=@requireddate,
							shippeddate=@shippeddate,shipperid=@shipperid,
							freight=@freight,shipname=@shipname,
							shipaddress=@shipaddress,shipcity=@shipcity,
							shipregion=@shipregion,shippostalcode=@shippostalcode,
							shipcountry=@shipcountry
							WHERE orderid=@orderid";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@custid", Convert.ToInt32(order.CustId)));
                cmd.Parameters.Add(new SqlParameter("@empid", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@requireddate", order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@shippeddate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipperName));
                cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));
                cmd.Parameters.Add(new SqlParameter("@orderid", id));
                conn.Close();
            }
        }
        */
        /// <summary>
        /// 刪除訂單
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
