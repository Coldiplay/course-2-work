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
    public class AccountsDB : ISampleDB
    {
        public List<Account> GetAccounts(string filter)
        {
            List<Account> accounts = new();
            if (DbConnection.GetDbConnection() == null)
                return accounts;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand("SELECT `a`.`ID`, `a`.`Title`, `a`.`Balance`, `a`.`CurrencyID`, `a`.`BankID`, `b`.`Title` BankTitle FROM `Accounts` a LEFT JOIN `Banks` b ON `a`.`BankID` = `b`.`ID` WHERE `a`.`Title` LIKE '%@filter%' OR `a`.`Balance` LIKE '%@filter%' OR `b`.`Title` LIKE '%@filter%';"))
            {
                using (var cmd2 = DbConnection.GetDbConnection().CreateCommand(""))
                { }
                    cmd.Parameters.Add(new MySqlParameter("filter", filter));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var account = new Account
                            {
                                ID = dr.GetInt32("ID"),
                                Title = dr.GetString("Title"), //dr.IsDBNull("Title") ? null : dr.GetString("Title"),
                                Balance = dr.GetDecimal("Balance"),
                                CurrencyID = dr.GetInt32("CurrencyID"),
                                BankID = dr.IsDBNull("BankID") ? null : dr.GetInt32("BankID")
                            };
                            if (account.BankID != null)
                            {
                                account.Bank = new Bank
                                {
                                    ID = (int)account.BankID,
                                    Title = dr.GetString("BankTitle")
                                };
                            }
                            accounts.Add(account);
                        }
                    }
                });
                DbConnection.GetDbConnection().CloseConnection();
            }
            return accounts;
        }


        public bool Insert(IModel acc)
        {
            Account account = acc as Account;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (MySqlCommand cmd = DbConnection.GetDbConnection().CreateCommand("INSERT INTO `Accounts` VALUES(0, @title, @balance, @currencyId, @bankId); SELECT LAST_INSERT_ID();"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", account.Title));
                cmd.Parameters.Add(new MySqlParameter("balance", account.Balance));
                cmd.Parameters.Add(new MySqlParameter("currencyId", account.CurrencyID));
                cmd.Parameters.Add(new MySqlParameter("bankId", account.BankID));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        account.ID = id;
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

        public bool Remove(IModel acc)
        {
            Account account = acc as Account;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"DELETE FROM `Accounts` WHERE ID = {account.ID}"))
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

        public bool Update(IModel acc)
        {
            Account account = acc as Account;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"UPDATE `Accounts` set `Title`=@title, `Balance`=@balance, `CurrencyID`=@currencyId, `BankID`=@bankId WHERE `ID` = {account.ID};"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", account.Title));
                cmd.Parameters.Add(new MySqlParameter("balance", account.Balance));
                cmd.Parameters.Add(new MySqlParameter("currencyId", account.CurrencyID));
                cmd.Parameters.Add(new MySqlParameter("bankID", account.BankID));

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
