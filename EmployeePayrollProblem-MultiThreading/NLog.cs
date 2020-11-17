// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NLog.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Akash Kumar Singh"/>
// --------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollProblem_MultiThreading
{
    public class NLog
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// Method to log for the debug operation
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
        /// Method to log for the message at error level
        public void LogError(string message)
        {
            logger.Error(message);
        }
        /// Method to log for the message at information level
        public void LogInfo(string message)
        {
            logger.Info(message);
        }
        /// Method to log for the message at warning level
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
