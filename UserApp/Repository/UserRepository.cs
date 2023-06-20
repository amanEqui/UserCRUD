using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UserApp.Models;

namespace UserApp.Repository
{
   
        public class UserRepository
        {
            private readonly string connectionString;

            public UserRepository(string connectionString)
            {
                this.connectionString = connectionString;
            }

            public IEnumerable<User> GetAllUsers()
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM [User] where Status=1";
                    var users = connection.Query<User>(query).ToList();

        
                    foreach (var user in users)
                    {
                        user.Skills = GetSkillsForUser(connection, user.UserID);
                    }

                    return users;
                }
            }

            public User GetUserByID(int userId)
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM [User] WHERE UserID = @UserId";
                    var user = connection.Query<User>(query, new { UserId = userId }).FirstOrDefault();

                    if (user != null)
                    {
                        user.Skills = GetSkillsForUser(connection, user.UserID);
                    }

                    return user;
                }
            }

            public void InsertUser(User user)
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO [User] (UserName, UserPass, DOB, Gender, Status, Phone, Address, Email)
                VALUES (@UserName, @UserPass, @DOB, @Gender, @Status, @Phone, @Address, @Email);
                SELECT CAST(SCOPE_IDENTITY() as int)";
                    int userId = connection.Query<int>(query, user).Single();
                    user.UserID = userId;
                    SaveUserSkills(connection, user);
                }
            }





        public void UpdateUser(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE [User] SET UserName = @UserName, UserPass = @UserPass, DOB = @DOB, Gender = @Gender, Phone = @Phone, Address = @Address, Email = @Email WHERE UserID = @UserID";

                connection.Execute(query, user);
             }

        }

      

        public void UpdateUserSkills(int userId, List<int> selectedSkills)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

              
                var deleteQuery = "DELETE FROM UserSkills WHERE UserID = @UserId";
                connection.Execute(deleteQuery, new { UserId = userId });

                var insertQuery = "INSERT INTO UserSkills (UserID, SkillID) VALUES (@UserId, @SkillId)";
                if (selectedSkills != null && selectedSkills.Any())
                {
                    var parameters = selectedSkills.Select(skillId => new { UserId = userId, SkillId = skillId });
                    connection.Execute(insertQuery, parameters);
                }
            }
        }





        public void DeleteUser(int userId)
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                  string deleteSkillsQuery = "DELETE FROM UserSkills WHERE UserID = @UserId";
                   connection.Execute(deleteSkillsQuery, new { UserId = userId });
                   string query = "DELETE FROM [User] WHERE UserID = @UserId";
                    connection.Execute(query, new { UserId = userId });

               
                    DeleteUserSkills(connection, userId);
                }
            }

            private List<Skill> GetSkillsForUser(IDbConnection connection, int userId)
            {
                string query = @"
            SELECT Skills.*
            FROM Skills
            INNER JOIN UserSkills ON Skills.SkillID = UserSkills.SkillID
            WHERE UserSkills.UserID = @UserId";
                return connection.Query<Skill>(query, new { UserId = userId }).ToList();
            }

            private void SaveUserSkills(IDbConnection connection, User user)
            {
                
                DeleteUserSkills(connection, user.UserID);

             
                if (user.Skills != null && user.Skills.Any())
                {
                    string query = "INSERT INTO UserSkills (UserID, SkillID) VALUES (@UserID, @SkillID)";
                    connection.Execute(query, user.Skills.Select(skill => new { UserID = user.UserID, SkillID = skill.SkillID }));
                }
            }

            private void DeleteUserSkills(IDbConnection connection, int userId)
            {
                string query = "DELETE FROM UserSkills WHERE UserID = @UserId";
                connection.Execute(query, new { UserId = userId });
            }

        public void InsertUserSkills(int userId, List<int> skillIds)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var skillId in skillIds)
                {
                    string query = "INSERT INTO UserSkills (UserID, SkillID) VALUES (@UserId, @SkillId)";
                    connection.Execute(query, new { UserId = userId, SkillId = skillId });
                }
            }
        }


        public int InsertUserAndGetID(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
            INSERT INTO [User] (UserName, UserPass, DOB, Gender, Phone, Address, Email)
            VALUES (@UserName, @UserPass, @DOB, @Gender, @Phone, @Address, @Email);
            SELECT CAST(SCOPE_IDENTITY() as int);";

                int userId = connection.ExecuteScalar<int>(query, user);
                return userId;
            }
        }


     


        public List<Skill> GetSkillsByUserId(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT s.* FROM Skills s INNER JOIN UserSkills us ON s.SkillID = us.SkillID WHERE us.UserID = @UserId";
                return connection.Query<Skill>(query, new { UserId = userId }).ToList();
            }
        }

        public List<Skill> GetAllSkills()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Skill>("SELECT * FROM Skills").ToList();
            }
        }

        public void Restore(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "Update [User] set Status = 0 where UserID = @UserID";
                connection.Execute(query, new { UserID = userId });
            }

        }

        public IEnumerable<User> RestoreUser()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [User] where Status=0";
                var users = connection.Query<User>(query).ToList();

                // Fetch related skills for each user
                foreach (var user in users)
                {
                    user.Skills = GetSkillsForUser(connection, user.UserID);
                }

                return users;
            }
        }

        public void Recover(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "Update [User] set Status = 1 where UserID = @UserID";
                connection.Execute(query, new { UserID = userId });
            }

        }


      

    }

}
