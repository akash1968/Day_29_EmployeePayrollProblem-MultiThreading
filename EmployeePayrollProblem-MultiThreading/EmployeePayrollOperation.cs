// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmployeePayrollOperation.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Akash Kumar Singh"/>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeePayrollProblem_MultiThreading
{
    public class EmployeePayrollOperation
    {
        /// Ensuring the established connection using the Sql Connection specifying the property.
        public static SqlConnection connection { get; set; }
        /// <summary>
        /// UC1 Adding the multiple employees record to data base without the threads.
        /// </summary>
        /// <param name="employeeList"></param>
        /// <returns></returns>
        public bool AddEmployeeListToDataBase(List<EmployeeModel> employeeList)
        {
            foreach (var employee in employeeList)
            {
                Console.WriteLine("Employeee being added :", employee.EmployeeName);
                bool flag = AddEmployeeToDataBase(employee);
                Console.WriteLine("Employee Added :", employee.EmployeeName);
                if (flag == false)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// UC2 Adding the multiple employees record to data base with random threads allocation from task thread pool.
        /// </summary>
        /// <param name="employeeList"></param>
        public void AddEmployeeListToEmployeePayrollDataBaseWithThread(List<EmployeeModel> employeelList)
        {
            ///For each employeeData present in list new thread is created and all threads run according to the time slot assigned by the thread scheduler.
            employeelList.ForEach(employeeData =>
            {
                Task thread = new Task(() =>
                {
                    Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                    /// Printing the current thread id being utilised
                    Console.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
                    /// Calling the method to add the data to the address book database
                    this.AddEmployeeToDataBase(employeeData);
                    /// Indicating mesasage to end of data addition
                    Console.WriteLine("Employee added:" + employeeData.EmployeeName);
                });
                thread.Start();
            });
        }
        /// <summary>
        /// Adding Employee To Database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddEmployeeToDataBase(EmployeeModel emp)
        { /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    //Creating a stored Procedure for adding employees into database
                    SqlCommand command = new SqlCommand("dbo.SpAddEmployeeDetails", connection);
                    //Command type is set as stored procedure
                    command.CommandType = CommandType.StoredProcedure;
                    //Adding values from employeemodel to stored procedure using disconnected architecture
                    //connected architecture will only read the data
                    command.Parameters.AddWithValue("@EmpName", emp.EmployeeName);
                    command.Parameters.AddWithValue("@StartDate", emp.StartDate);
                    command.Parameters.AddWithValue("@gender", emp.Gender);
                    command.Parameters.AddWithValue("@PhoneNo", emp.PhoneNumber);
                    command.Parameters.AddWithValue("@address", emp.Address);
                    command.Parameters.AddWithValue("@department", emp.Department);
                    command.Parameters.AddWithValue("@BasicPay", emp.BasicPay);
                    command.Parameters.AddWithValue("@deductions", emp.Deductions);
                    command.Parameters.AddWithValue("@TaxablePay", emp.TaxablePay);
                    command.Parameters.AddWithValue("@Income_Tax", emp.Tax);
                    command.Parameters.AddWithValue("@NetPay", emp.NetPay);
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    connection.Close();

                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
