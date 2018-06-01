using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MachineInformationApp.Interfaces;

namespace MachineInformationApp
{
    public class AgentDataService : IAgentDataService
    {
        private readonly string _connectionString;

        public AgentDataService(string dbName)
        {
            _connectionString = $"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog={dbName}";
        }

        public AgentDataService()
        {
            _connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=SkyNetPortalDB";
        }

        public IEnumerable<AgentExecution> GetExecutedAgentDetails()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * " +
                    "FROM dbo.AgentExecution " +
                    "where ExecutionId=01 ";
                var agents = connection.Query<AgentExecution>(sql);
                return agents;
            }

        }

        public IEnumerable<AgentExecution> GetExecutedListAgentDetails()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * " +
                    "FROM dbo.AgentExecution ";
                var AllAgents = connection.Query<AgentExecution>(sql);
                return AllAgents;
            }
        }

        public int GetInsertedData(AgentExecution agent)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "insert into AgentExecution(Command, Result, ExecutionTime) Values(@Command, @Result, @ExecutionTime)";
                return connection.Execute(sql, agent);
            }
        }
    }
}