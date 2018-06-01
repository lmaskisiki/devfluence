using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MachineInformationApp.Interfaces;
namespace MachineInformationApp
{
    public class DashboardDataService : IDashboardDataService
    {
        private readonly string _connectionString;

        public DashboardDataService(string dbName)
        {
            _connectionString = $"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog={dbName}";
        }

        public DashboardDataService()
        {
            _connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=SkyNetPortalDB";
        }

        public IEnumerable<DashboardExecution> GetDashboardDetails()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM dbo.Dashboard WHERE ID = 1 ";
                var agents = connection.Query<DashboardExecution>(sql);
                return agents;
            }

        }

        public IEnumerable<DashboardExecution> GetNumberOfDashboardDetails()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM dbo.Dashboard";
                var agents = connection.Query<DashboardExecution>(sql);
                return agents;
            }

        }

        public int GetInsertDetails(DashboardExecution activity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql= "insert into Dashboard (Target, Action, ActionResult, ExecutionTime) Values (@Target, @Action, @ActionResult, @ExecutionTime)";
               return  connection.Execute(sql, activity);
             }
        }
    }
}
