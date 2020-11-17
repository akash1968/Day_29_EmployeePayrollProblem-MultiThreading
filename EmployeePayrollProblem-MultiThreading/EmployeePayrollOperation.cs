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
        // Adding NLog Class to feed the log details for proper monitoring
        NLog nLog = new NLog();
        /// Mutex is a synchronization primitive to implement interthread execution synchronization
        /// Which means a thread can be locked i.e. Untill the thread completes it's execution new thread will not be permitted the entry
        private static Mutex threadMute = new Mutex();
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
                nLog.LogDebug("Adding the Employee: " + employee.EmployeeName + "via ThreadID: " + Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Employeee being added :", employee.EmployeeName);
                bool flag = AddEmployeeToDataBase(employee);
                Console.WriteLine("Employee Added :", employee.EmployeeName);
                nLog.LogInfo("Employee Successfully added in Database via ThreadId: " + Thread.CurrentThread.ManagedThreadId);
                if (flag == false)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// UC2 Adding the multiple employees record to data base with random threads allocation from task thread pool.
        /// </summary>
        /// <param name="employeeList"></param>
        public void AddEmployeeListToEmployeePayrollDataBaseWithThread(List<EmployeeModel> employeeList)
        {
            ///For each employeeData present in list new thread is created and all threads run according to the time slot assigned by the thread scheduler.
            employeeList.ForEach(employeeData =>
            {
                nLog.LogDebug("Adding the Employee: " + employeeData.EmployeeName + "via ThreadID: " + Thread.CurrentThread.ManagedThreadId);
                Task thread = new Task(() =>
                {
                    Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                    /// Printing the current thread id being utilised
                    Console.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
                    /// Calling the method to add the data to the address book database
                    this.AddEmployeeToDataBase(employeeData);
                    /// Indicating mesasage to end of data addition
                    Console.WriteLine("Employee added:" + employeeData.EmployeeName);
                    nLog.LogInfo("Employee Successfully added in Database via ThreadId: " + Thread.CurrentThread.ManagedThreadId);
                });
                thread.Start();
            });
        }
        /// <summary>
        ///  UC3 Adding the multiple employees record to data base with synchronised threads allocation from task thread pool.
        /// </summary>
        /// <param name="employeeList"></param>
        public void AddEmployeeListToDataBaseWithThreadSynchronization(List<EmployeeModel> employeeList)
        {
            ///For each employeeData present in list new thread is created and all threads run according to the time slot assigned by the thread scheduler.
            employeeList.ForEach(employeeData =>
            {
                nLog.LogDebug("Adding the Employee: " + employeeData.EmployeeName + "via ThreadID: " + Thread.CurrentThread.ManagedThreadId);
                Task thread = new Task(() =>
                {
                    //Lock the set of codes for the current employeeData
                    lock (employeeData)
                    {
                         Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                        /// Printing the current thread id being utilised
                        Console.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
                        /// Calling the method to add the data to the address book database
                        this.AddEmployeeToDataBase(employeeData);
                        /// Indicating mesasage to end of data addition
                        Console.WriteLine("Employee added:" + employeeData.EmployeeName);
                        nLog.LogInfo("Employee Successfully added in Database via ThreadId: " + Thread.CurrentThread.ManagedThreadId);
                        
                    }

                });
                thread.Start();
                thread.Wait();
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
