namespace dealstealunreal.com.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using Exceptions;
    using Interfaces;
    using Models.Deals;
    using Ninject.Extensions.Logging;

    public class DealDataAccess : IDealDataAccess
    {
        private const string GetAllDealsQuery = "select * from Deals";
        private const string GetDealFromIdQuery = "select * from Deals where DealId = @dealId";
        private const string GetDealFromNameQuery = "select * from Deals where Title like @dealTitle";
        private const string SaveDealQuery = "insert into Deals (Username, Title, Description, Retailer, Url, Price, ImageUrl, Date, Active) values(@userName, @title, @description, @retailer, @url, @price, @imageUrl, @date, @active)";
        private const string SaveDescriptionQuery = "update deals set Description = @description where DealId = @dealId";
        private const string SaveActiveQuery = "update deals set Active = @active where DealId = @dealId";
        private const string DeleteDealQuery = "delete from deals where dealId = @dealId";
        private ILogger log;

        public DealDataAccess(ILogger log)
        {
            this.log = log;
        }

        public IList<Deal> GetAllDeals()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadonlyDatabase"].ConnectionString;

            List<Deal> deals = new List<Deal>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = GetAllDealsQuery;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var deal = GetDeal(reader);

                                deals.Add(deal);
                            }
                        }
                    }
                }

                return deals;
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not load all deals");
            }

            throw new DealDatabaseException();
        }

        public Deal GetDeal(int dealId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadonlyDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = GetDealFromIdQuery;
                        command.Parameters.AddWithValue("@dealId", dealId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return GetDeal(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not get deal with id {0}", dealId);
            }

            throw new DealDatabaseException();
        }

        public IList<Deal> SearchForDeal(string dealName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadonlyDatabase"].ConnectionString;

            List<Deal> deals = new List<Deal>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = GetDealFromNameQuery;
                        command.Parameters.AddWithValue("@dealTitle", "%" + dealName + "%");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var deal = GetDeal(reader);

                                deals.Add(deal);
                            }
                        }
                    }
                }

                return deals;
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not load deal with name similar to {0}", dealName);
            }

            throw new DealDatabaseException();
        }

        public void SaveDeal(Deal deal)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = SaveDealQuery;

                        command.Parameters.AddWithValue("@active", true);
                        command.Parameters.AddWithValue("@date", DateTime.Now);
                        command.Parameters.AddWithValue("@description", deal.Description ?? string.Empty);
                        command.Parameters.AddWithValue("@imageUrl", deal.ImageUrl);
                        command.Parameters.AddWithValue("@price", deal.Price);
                        command.Parameters.AddWithValue("@retailer", deal.Retailer);
                        command.Parameters.AddWithValue("@title", deal.Title);
                        command.Parameters.AddWithValue("@url", deal.Url);
                        command.Parameters.AddWithValue("@username", deal.UserName);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not save deal - title: {0} description: {1} url: {2} image: {3} user: {4}", deal.Title, deal.Description, deal.Url, deal.ImageUrl, deal.UserName);
            }
        }

        public void SaveDealDescription(int dealId, string description)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = SaveDescriptionQuery;

                        command.Parameters.AddWithValue("@dealId", dealId);
                        command.Parameters.AddWithValue("@description", description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not save deal {0} description {1}", dealId, description);
            }
        }

        public void SaveDealActive(int dealId, bool active)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = SaveActiveQuery;

                        command.Parameters.AddWithValue("@dealId", dealId);
                        command.Parameters.AddWithValue("@active", active);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not save deal {0} active {1}", dealId, active);
            }
        }

        public void DeleteDeal(int dealId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = DeleteDealQuery;

                        command.Parameters.AddWithValue("@dealId", dealId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not delete deal {0}", dealId);
            }
        }

        private static Deal GetDeal(SqlDataReader reader)
        {
            Deal deal = new Deal
            {
                Title = reader.GetString(reader.GetOrdinal("Title")).Trim(),
                DealID = reader.GetInt32(reader.GetOrdinal("DealId")),
                UserName = reader.GetString(reader.GetOrdinal("Username")).Trim(),
                Description = reader.GetString(reader.GetOrdinal("Description")).Trim(),
                Retailer = reader.GetString(reader.GetOrdinal("Retailer")).Trim(),
                Price = Math.Round(reader.GetFloat(reader.GetOrdinal("Price")), 2),
                Url = reader.GetString(reader.GetOrdinal("Url")).Trim(),
                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")).Trim(),
                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                Active = reader.GetBoolean(reader.GetOrdinal("Active"))
            };

            return deal;
        }
    }
}