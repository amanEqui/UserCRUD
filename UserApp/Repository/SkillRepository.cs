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
        public class SkillRepository
        {
            private readonly string connectionString;

            public SkillRepository(string connectionString)
            {
                this.connectionString = connectionString;
            }

            public IEnumerable<Skill> GetAllSkills()
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Skills";
                    var skills = connection.Query<Skill>(query).ToList();

                    // Fetch related users for each skill
                    foreach (var skill in skills)
                    {
                        skill.Users = GetUsersForSkill(connection, skill.SkillID);
                    }

                    return skills;
                }
            }

            public Skill GetSkillByID(int skillId)
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Skills WHERE SkillID = @SkillId";
                    var skill = connection.Query<Skill>(query, new { SkillId = skillId }).FirstOrDefault();

                    if (skill != null)
                    {
                        // Fetch related users for the skill
                        skill.Users = GetUsersForSkill(connection, skill.SkillID);
                    }

                    return skill;
                }
            }

            public void InsertSkill(Skill skill)
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO Skills (SkillName)
                VALUES (@SkillName);
                SELECT CAST(SCOPE_IDENTITY() as int)";
                    int skillId = connection.Query<int>(query, skill).Single();
                    skill.SkillID = skillId;
                }
            }

            public void UpdateSkill(Skill skill)
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Skills SET SkillName = @SkillName WHERE SkillID = @SkillID";
                    connection.Execute(query, skill);
                }
            }

            public void DeleteSkill(int skillId)
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Skills WHERE SkillID = @SkillId";
                    connection.Execute(query, new { SkillId = skillId });
                }
            }

            private List<User> GetUsersForSkill(IDbConnection connection, int skillId)
            {
                string query = @"
            SELECT [User].*
            FROM [User]
            INNER JOIN UserSkills ON [User].UserID = UserSkills.UserID
            WHERE UserSkills.SkillID = @SkillId";
                return connection.Query<User>(query, new { SkillId = skillId }).ToList();
            }

        public List<Skill> GetSkillsByUserId(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT s.SkillID, s.SkillName
            FROM Skills AS s
            INNER JOIN UserSkills AS us ON s.SkillID = us.SkillID
            WHERE us.UserID = @UserId";

                return connection.Query<Skill>(query, new { UserId = userId }).ToList();
            }
        }


    }

}