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
    public class DebtsDB : ISampleDB
    {
        public List<Debt> GetDebts(string filter)
        {
            List<Debt> debts = new();
            if (DbConnection.GetDbConnection() == null)
                return debts;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand("SELECT `ID`, `Title`, `Summ`, `AnnualInterest`, `DateOfPick`, `DateOfReturn`, `CurrencyID` FROM `Debts` WHERE `Title` LIKE '%@filter%' OR `Summ` LIKE '%@filter%' OR `DateOfPick` LIKE '%@filter%' OR `DateOfReturn` LIKE '%@filter%' OR `AnnualInterest` LIKE '%@filter%'"))
            {
                cmd.Parameters.Add(new MySqlParameter("filter", filter));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            debts.Add(new Debt
                            {
                                ID = dr.GetInt32("ID"),
                                Title = dr.IsDBNull("Title") ? null : dr.GetString("Title"),
                                Summ = dr.GetDecimal("Summ"),
                                AnnualInterest = dr.GetInt16("AnnualInterest"),
                                DateOfPick = dr.GetDateOnly("DateOfPick").ToDateTime(new TimeOnly()),
                                DateOfReturn = dr.GetDateOnly("DateOfReturn").ToDateTime(new TimeOnly()),
                                CurrencyID = dr.GetInt32("CurrencyID")
                            });
                        }
                    }

                });
                DbConnection.GetDbConnection().CloseConnection();
            }
            return debts;
        }


        public bool Insert(IModel deb)
        {
            Debt debt = deb as Debt;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (MySqlCommand cmd = DbConnection.GetDbConnection().CreateCommand("INSERT INTO `Debts` VALUES(0, @title, @summ, @annualInterest, @dateOfPick, @dateOfReturn,  @currencyId); SELECT LAST_INSERT_ID();"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", debt.Title));
                cmd.Parameters.Add(new MySqlParameter("summ", debt.Summ));
                cmd.Parameters.Add(new MySqlParameter("annualInterest", debt.AnnualInterest));
                cmd.Parameters.Add(new MySqlParameter("dateOfPick", debt.DateOfPick));
                cmd.Parameters.Add(new MySqlParameter("dateOfReturn", debt.DateOfReturn));
                cmd.Parameters.Add(new MySqlParameter("currencyId", debt.CurrencyID));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        debt.ID = id;
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

        public bool Remove(IModel deb)
        {
            var debt = deb as Debt;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"DELETE FROM `Debts` WHERE ID = {debt.ID}"))
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

        public bool Update(IModel deb)
        {
            var debt = deb as Debt;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"UPDATE `Debts` set `Title`=@title, `Summ`=@summ, `AnnualInterest`=@annualInterest, `DateOfPick`=@dateOfPick, `DateOfReturn`=@dateOfReturn, `CurrencyID`=@currencyId WHERE `ID` = {debt.ID} ;"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", debt.Title));
                cmd.Parameters.Add(new MySqlParameter("summ", debt.Summ));
                cmd.Parameters.Add(new MySqlParameter("AnnualInterest", debt.AnnualInterest));
                cmd.Parameters.Add(new MySqlParameter("DateOfPick", debt.DateOfPick));
                cmd.Parameters.Add(new MySqlParameter("DateOfReturn", debt.DateOfReturn));
                cmd.Parameters.Add(new MySqlParameter("currencyId", debt.CurrencyID));

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
