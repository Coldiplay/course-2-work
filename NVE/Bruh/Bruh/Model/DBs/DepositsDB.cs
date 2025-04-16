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
    public class DepositsDB : ISampleDB
    {
        public List<Deposit> GetDeposits(string filter)
        {
            List<Deposit> deposits = new();
            if (DbConnection.GetDbConnection() == null)
                return deposits;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand("SELECT `ID`, `Title`, `InitalSumm`, `DateOfOpening`, `DateOfClosing`, `Capitalization`, `InterestRate`, `PeriodicityOfPayment`, `BankID`, `TypeOfDepositID`, `CurrencyID` FROM `Deposits` WHERE `Title` LIKE '%@filter%' OR SummOfDeposit LIKE '%@filter%' OR `DateOfOpening` LIKE '%@filter%' OR `DateOfClosing` LIKE '%@filter%'"))
            {
                cmd.Parameters.Add(new MySqlParameter("filter", filter));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            deposits.Add(new Deposit
                            {
                                ID = dr.GetInt32("ID"),
                                Title = dr.IsDBNull("Title") ? null : dr.GetString("Title"),
                                InitalSumm = dr.GetDecimal("InitalSumm"),
                                OpenDate = dr.GetDateOnly("DateOfOpening").ToDateTime(new TimeOnly()),
                                CloseDate = dr.GetDateOnly("DateOfClosing").ToDateTime(new TimeOnly()),
                                Capitalization = dr.GetBoolean("Capitalization"),
                                InterestRate = dr.GetInt32("InterestRate"),
                                PeriodicityOfPayment = dr.GetString("PeriodicityOfPayment"),
                                BankID = dr.GetInt32("BankID"),
                                TypeOfDepositID = dr.GetInt32("TypeOfDepositID"),
                                CurrencyID = dr.GetInt32("CurrencyID")
                            });
                        }
                    }
                });
                DbConnection.GetDbConnection().CloseConnection();
            }
            return deposits;
        }


        public bool Insert(IModel depos)
        {
            Deposit deposit = depos as Deposit;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (MySqlCommand cmd = DbConnection.GetDbConnection().CreateCommand("INSERT INTO `Deposits` VALUES(0, @title, @initalSumm, @dateOfOpening, @dateOfClosing, @capitalization, @interestRate, @periodicityOfPayment, @bankID, @typeOfDeposit, @currencyId); SELECT LAST_INSERT_ID();"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", deposit.Title));
                cmd.Parameters.Add(new MySqlParameter("initalSumm", deposit.InitalSumm));
                cmd.Parameters.Add(new MySqlParameter("dateOfOpening", deposit.OpenDate));
                cmd.Parameters.Add(new MySqlParameter("dateOfClosing", deposit.CloseDate));
                cmd.Parameters.Add(new MySqlParameter("capitalization", deposit.Capitalization));
                cmd.Parameters.Add(new MySqlParameter("interestRate", deposit.InterestRate));
                cmd.Parameters.Add(new MySqlParameter("periodicityOfPayment", deposit.PeriodicityOfPayment));
                cmd.Parameters.Add(new MySqlParameter("bankID", deposit.BankID));
                cmd.Parameters.Add(new MySqlParameter("typeOfDeposit", deposit.TypeOfDepositID));
                cmd.Parameters.Add(new MySqlParameter("currencyId", deposit.CurrencyID));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        deposit.ID = id;
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

        public bool Remove(IModel depos)
        {
            Deposit deposit = depos as Deposit;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"DELETE FROM `Debts` WHERE ID = {deposit.ID}"))
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

        public bool Update(IModel depos)
        {
            Deposit deposit = depos as Deposit;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"UPDATE `Deposits` set `Title`=@title, `InitalSumm`=@initalSumm, `DateOfOpening`=@dateOfOpening, `DateOfClosing`=@dateOfClosing, `Capitalization`=@capitalization, `InterestRate`=@interestRate, `PeriodicityOfPayment`=@periodicityOfPayment, `BankID`=@bankId, `TypeOfDepositID`=@typeOfDeposit, `CurrencyID`=@currencyId WHERE `ID`={deposit.ID};"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", deposit.Title));
                cmd.Parameters.Add(new MySqlParameter("initalSumm", deposit.InitalSumm));
                cmd.Parameters.Add(new MySqlParameter("dateOfOpening", deposit.OpenDate));
                cmd.Parameters.Add(new MySqlParameter("dateOfClosing", deposit.CloseDate));
                cmd.Parameters.Add(new MySqlParameter("capitalization", deposit.Capitalization));
                cmd.Parameters.Add(new MySqlParameter("interestRate", deposit.InterestRate));
                cmd.Parameters.Add(new MySqlParameter("periodicityOfPayment", deposit.PeriodicityOfPayment));
                cmd.Parameters.Add(new MySqlParameter("bankID", deposit.BankID));
                cmd.Parameters.Add(new MySqlParameter("typeOfDeposit", deposit.TypeOfDepositID));
                cmd.Parameters.Add(new MySqlParameter("currencyId", deposit.CurrencyID));

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
