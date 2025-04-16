using Bruh.Model.Models;
using Bruh.VMTools;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bruh.Model.DBs
{
    public class OperationsDB : ISampleDB
    {
        public List<Operation> GetOperations(string filter)
        {
            List<Operation> operations = new();
            if (DbConnection.GetDbConnection() == null)
                return operations;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand("SELECT `ID`, `Title`, `Cost`, `TransactDate`, `DateOfCreate`, `Income`, `Description`, `PeriodicityID`, `CategoryID`, `DebtID`, `BankAccountID` FROM `Operations` WHERE `Title` LIKE '%@filter%' OR `Cost` LIKE '%@filter%' OR `Description` LIKE '%@filter%' OR `TransactDate` LIKE '%@filter%' "))
            {
                cmd.Parameters.Add(new MySqlParameter("filter", filter));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            operations.Add(new Operation
                            {
                                ID = dr.GetInt32("ID"),
                                Title = dr.GetString("Title"),
                                Cost = dr.GetDecimal("Cost"),
                                TransactDate = dr.GetDateOnly("TransactDate").ToDateTime(new TimeOnly()),
                                DateOfCreate = dr.GetDateOnly("DateOfCreate").ToDateTime(new TimeOnly()),
                                Income = dr.GetBoolean("Income"),
                                Description = dr.IsDBNull("Description") ? null : dr.GetString("Description"),
                                PeriodicityID = dr.IsDBNull("PeriodicityID") ? null : dr.GetInt32("PeriodicityID"),
                                CategoryID = dr.GetInt32("CategoryID"),
                                DebtID = dr.IsDBNull("DebtID") ? null : dr.GetInt32("DebtID"),
                                BankAccountID = dr.GetInt32("BankAccountID")
                            });
                        }                        
                    }

                });
                DbConnection.GetDbConnection().CloseConnection();
            }
                return operations;
        }


        public bool Insert(IModel oper)
        {
            var operation = oper as Operation;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (MySqlCommand cmd = DbConnection.GetDbConnection().CreateCommand("INSERT INTO `Operations` VALUES(0, @title, @cost, @transactDate, @DateOfCreate, @income, @description,  @periodicityId, @categotyId, @debtId, @bankAccountId); SELECT LAST_INSERT_ID();"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", operation.Title));
                cmd.Parameters.Add(new MySqlParameter("cost", operation.Cost));
                cmd.Parameters.Add(new MySqlParameter("transactDate", operation.TransactDate));
                cmd.Parameters.Add(new MySqlParameter("DateOfCreate", operation.DateOfCreate));
                cmd.Parameters.Add(new MySqlParameter("income", operation.Income));
                cmd.Parameters.Add(new MySqlParameter("description", operation.Description));
                cmd.Parameters.Add(new MySqlParameter("periodicityId", operation.PeriodicityID));
                cmd.Parameters.Add(new MySqlParameter("categotyId", operation.CategoryID));
                cmd.Parameters.Add(new MySqlParameter("debtId", operation.DebtID));
                cmd.Parameters.Add(new MySqlParameter("bankAccountId", operation.BankAccountID));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        operation.ID = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                });

                DbConnection.GetDbConnection().CloseConnection();
            }
            return result;
        }

        public bool Remove(IModel oper)
        {
            var operation = oper as Operation;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"DELETE FROM `Operations` WHERE ID = {operation.ID}"))
            {
                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                });
                DbConnection.GetDbConnection().CloseConnection();
            }
            return result;
        }

        public bool Update(IModel oper)
        {
            var operation = oper as Operation;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"UPDATE `Operations` set `Title`=@title, `Cost`=@cost, `TransactDate`=@transactDate, `DateOfCreate`=@dateOfCreate, `Income`=@income, `Description`=@description, `PeriodicityID`=@periodicityId, `CategoryID`=@categoryId, `DebtID`=@debtId, `BankAccountID`=@bankAccountId WHERE `ID`={operation.ID} ;"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", operation.Title));
                cmd.Parameters.Add(new MySqlParameter("cost", operation.Cost));
                cmd.Parameters.Add(new MySqlParameter("transactDate", operation.TransactDate));
                cmd.Parameters.Add(new MySqlParameter("DateOfCreate", operation.DateOfCreate));
                cmd.Parameters.Add(new MySqlParameter("income", operation.Income));
                cmd.Parameters.Add(new MySqlParameter("description", operation.Description));
                cmd.Parameters.Add(new MySqlParameter("periodicityId", operation.PeriodicityID));
                cmd.Parameters.Add(new MySqlParameter("categoryId", operation.CategoryID));
                cmd.Parameters.Add(new MySqlParameter("debtId", operation.DebtID));
                cmd.Parameters.Add(new MySqlParameter("bankAccountId", operation.BankAccountID));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                });
                DbConnection.GetDbConnection().CloseConnection();
            }
                return result;
        }
    }
}
