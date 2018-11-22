using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TwitterClone
{
    using System.Collections.Generic;

    using TwitterClone.Models;

    public class DatabaseOperation
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        public bool CreateUser(Person person)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            int result = 0;
            try
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO PERSON VALUES('" + person.UserId + "',HashBytes('MD5', '" + person.Password + "'),'" + person.FullName + "','" + person.Email + "',GETDATE(),'Y');";
                command.CommandType = CommandType.Text;
                connection.Open();
                result = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return result > 0;
        }

        public string SignIn(Person person)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            object result;
            try
            {
                command.Connection = connection;
                command.CommandText = "SELECT USER_ID FROM PERSON WHERE PASSWORD = HASHBYTES('MD5','" + person.Password + "') AND ACTIVE = 'Y';";
                command.CommandType = CommandType.Text;
                connection.Open();
                result = command.ExecuteScalar();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return (string)result;
        }

        public bool SaveTweet(string userName, string message)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            int result = 0;
            try
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO TWEET VALUES('" + userName + "','" + message + "',GETDATE());";
                command.CommandType = CommandType.Text;
                connection.Open();
                result = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return result > 0;
        }

        public List<Tweet> RetriveTweet(string userName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;
            List<Tweet> tweets = new List<Tweet>();
            try
            {
                command.Connection = connection;
                command.CommandText = "SELECT [USER_ID], [MESSAGE], CREATED FROM (SELECT [USER_ID], [MESSAGE], CREATED FROM TWEET WHERE [USER_ID] = '" + userName + "' UNION SELECT [USER_ID], [MESSAGE], CREATED FROM TWEET where user_id in (SELECT [FOLLOWING_ID] from FOLLOWING  WHERE [USER_ID] = '" + userName + "')) AS T ORDER BY CREATED DESC;";
                command.CommandType = CommandType.Text;
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tweets.Add(new Tweet() { User = reader["USER_ID"] as string, Message = reader["MESSAGE"] as string, Created = (DateTime) reader["CREATED"] });
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return tweets;
        }

        public int RetriveFollowers(string userName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            int following = 0;
            
            try
            {
                command.Connection = connection;
                command.CommandText = "SELECT COUNT([USER_ID]) FROM FOLLOWING WHERE [FOLLOWING_ID] = '" + userName + "';";
                command.CommandType = CommandType.Text;
                connection.Open();
                following = (int) command.ExecuteScalar();
                
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return following;
        }

        public int RetriveFollowing(string userName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            int following = 0;

            try
            {
                command.Connection = connection;
                command.CommandText = "SELECT COUNT([FOLLOWING_ID]) FROM FOLLOWING WHERE [USER_ID] = '" + userName + "';";
                command.CommandType = CommandType.Text;
                connection.Open();
                following = (int)command.ExecuteScalar();

                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return following;
        }

        public bool SearchUser(string userName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            int result = 0;
            try
            {
                command.Connection = connection;
                command.CommandText = "SELECT COUNT([USER_ID]) FROM PERSON WHERE [USER_ID] = '" + userName + "';";
                command.CommandType = CommandType.Text;
                connection.Open();
                result = (int) command.ExecuteScalar();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return result > 0;
        }

        public bool Follow(string userName, string following)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            int result = 0;
            try
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO FOLLOWING VALUES('" + userName + "','" + following + "');";
                command.CommandType = CommandType.Text;
                connection.Open();
                result = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return result > 0;
        }

        public Person RetrivePerson(string userName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;
            Person person = new Person();
            try
            {
                command.Connection = connection;
                command.CommandText = "SELECT [USER_ID], [PASSWORD], FULLNAME,  EMAIL, case when ACTIVE = 'Y' THEN 'FALSE' ELSE 'TRUE' END AS ACTIVE FROM PERSON WHERE [USER_ID] = '" + userName + "';";
                command.CommandType = CommandType.Text;
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    person = new Person() { UserId = reader["USER_ID"] as string, FullName = reader["FULLNAME"] as string, Email = reader["EMAIL"] as string, Active = Convert.ToBoolean(reader["ACTIVE"]) };
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return person;
        }

        public bool UpdateProfile(Person person)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;
            int result = 0;
            char delete = person.Active ? 'N' : 'Y';

            try
            {
                command.Connection = connection;
                command.CommandText = "UPDATE PERSON SET [PASSWORD] = HASHBYTES('MD5','" + person.Password + "'), FULLNAME = '" + person.FullName + "',  EMAIL = '" + person.Email + "', ACTIVE = '" + delete + "'  WHERE [USER_ID] = '" + person.UserId + "';";
                command.CommandType = CommandType.Text;
                connection.Open();
                result = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection = null;
                command = null;
            }

            return result > 0;
        }
    }
}