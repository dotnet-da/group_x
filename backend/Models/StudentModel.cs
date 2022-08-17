using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace backend
{
    public class Student
    {
        public int id_student { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }

        internal Database Db { get; set; }

        public Student()
        {
        }

        internal Student(Database db)
        {
            Db = db;
        }

        public async Task<Student> FindOneAsync(int id_Student)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `student` WHERE `id_student` = @id_Student";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_student",
                DbType = DbType.Int32,
                Value = id_Student,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `student`;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }


        private async Task<List<Student>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Student>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Student(Db)
                    {
                        id_student = reader.GetInt32(0),
                        fname = reader.GetString(1),
                        lname = reader.GetString(2)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Student` (`fname`, `lname`) VALUES (@fname, @lname);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id_student = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `student` SET `fname` = @fname, `lname` = @lname WHERE `id_Student` = @id_Student;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `student` WHERE `id_student` = @id_student;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_student",
                DbType = DbType.Int32,
                Value = id_student,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fname",
                DbType = DbType.String,
                Value = fname,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lname",
                DbType = DbType.String,
                Value = lname,
            });
        }
    }
}