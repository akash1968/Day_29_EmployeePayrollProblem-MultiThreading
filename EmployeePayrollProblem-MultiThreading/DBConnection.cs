// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DBConnection.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Akash Kumar Singh"/>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EmployeePayrollProblem_MultiThreading
{
    class DBConnection
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {

            ///Specifying the connection string from the sql server connection
            string connectionString = @"Data Source=DESKTOP-TSL9UFS;Initial Catalog=payroll_service;Integrated Security=True;User ID=akash;Password=1997";
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}