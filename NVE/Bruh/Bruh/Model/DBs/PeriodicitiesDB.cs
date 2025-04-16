using Bruh.Model.Models;
using Bruh.VMTools;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bruh.Model.DBs
{
    public class PeriodicitiesDB : ISampleDB
    {
        public List<Periodicity> GetPeriodicities()
        {
            List<Periodicity> periodicities = new();
            if (DbConnection.GetDbConnection() == null)
                return periodicities;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand("SELECT `Id`, `Title` FROM `Periodicities`;"))
            {
                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            periodicities.Add(new Periodicity
                            {
                                ID = dr.GetInt32("ID"),
                                Title = dr.GetString("Title")
                            });
                        }
                    }
                });
                DbConnection.GetDbConnection().CloseConnection();
            }
            return periodicities;
        }


        public bool Insert(IModel period)
        {
            Periodicity periodicity = period as Periodicity;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (MySqlCommand cmd = DbConnection.GetDbConnection().CreateCommand("INSERT INTO `Periodicities` VALUES(0, @title); SELECT LAST_INSERT_ID();"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", periodicity.Title));

                DbConnection.GetDbConnection().OpenConnection();
                ExeptionHandler.Try(() =>
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        periodicity.ID = id;
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

        public bool Remove(IModel period)
        {
            Periodicity periodicity = period as Periodicity;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"DELETE FROM `Periodicities` WHERE ID = {periodicity.ID}"))
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

        public bool Update(IModel period)
        {
            Periodicity periodicity = period as Periodicity;
            bool result = false;
            if (DbConnection.GetDbConnection() == null)
                return result;

            using (var cmd = DbConnection.GetDbConnection().CreateCommand($"UPDATE `Periodicities` set `Title`=@title WHERE `ID` = {periodicity.ID};"))
            {
                cmd.Parameters.Add(new MySqlParameter("title", periodicity.Title));

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
