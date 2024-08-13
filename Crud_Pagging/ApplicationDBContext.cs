//using Crud_Pagging.Models;
//using System.Data;
//using System.Data.SqlClient;

//namespace Crud_Pagging
//{
//    public class ApplicationDBContext
//    {
//        private readonly string _connectionString = "Data Source=SHAHM-LAP;Initial Catalog=Empl SP DB;Integrated Security=True";

//        public SqlConnection GetConnection()
//        {
//            return new SqlConnection(_connectionString);
//        }

//        private DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
//        {
//            using (SqlConnection con = GetConnection())
//            using (SqlCommand cmd = new SqlCommand(query, con))
//            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
//            {
//                if (parameters != null)
//                {
//                    cmd.Parameters.AddRange(parameters);
//                }

//                DataTable dt = new DataTable();
//                adapter.Fill(dt);
//                return dt;
//            }
//        }

//        private int ExecuteNonQuery(string storedProcedure, SqlParameter[] parameters)
//        {
//            using (SqlConnection con = GetConnection())
//            using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
//            {
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.AddRange(parameters);

//                con.Open();
//                return cmd.ExecuteNonQuery();
//            }
//        }

//        public List<StudentModel> GetAllStudents()
//        {
//            DataTable dt = ExecuteQuery("SELECT * FROM students");
//            return dt.AsEnumerable().Select(row => new StudentModel
//            {
//                Stu_Id = Convert.ToInt32(row["Stu_Id"]),
//                FirstName = row["FirstName"].ToString(),
//                LastName = row["LastName"].ToString(),
//                Age = Convert.ToInt32(row["Age"]),
//                Gender = row["Gender"].ToString()
//            }).ToList();
//        }

//        public StudentModel GetStudentById(int id)
//        {
//            SqlParameter[] parameters = { new SqlParameter("@Stu_Id", id) };
//            DataTable dt = ExecuteQuery("SELECT * FROM students WHERE Stu_Id = @Stu_Id", parameters);

//            if (dt.Rows.Count == 1)
//            {
//                DataRow row = dt.Rows[0];
//                return new StudentModel
//                {
//                    Stu_Id = Convert.ToInt32(row["Stu_Id"]),
//                    FirstName = row["FirstName"].ToString(),
//                    LastName = row["LastName"].ToString(),
//                    Age = Convert.ToInt32(row["Age"]),
//                    Gender = row["Gender"].ToString()
//                };
//            }
//            return null;
//        }

//        public bool CreateStudent(StudentModel student)
//        {
//            SqlParameter[] parameters = {
//                new SqlParameter("@FirstName", student.FirstName),
//                new SqlParameter("@LastName", student.LastName),
//                new SqlParameter("@Age", student.Age),
//                new SqlParameter("@Gender", student.Gender)
//            };

//            int result = ExecuteNonQuery("spcreatestudent", parameters);
//            return result > 0;
//        }

//        public bool UpdateStudent(StudentModel student)
//        {
//            SqlParameter[] parameters = {
//                new SqlParameter("@Stu_Id", student.Stu_Id),
//                new SqlParameter("@FirstName", student.FirstName),
//                new SqlParameter("@LastName", student.LastName),
//                new SqlParameter("@Age", student.Age),
//                new SqlParameter("@Gender", student.Gender)
//            };

//            int result = ExecuteNonQuery("spupdatestudent", parameters);
//            return result > 0;
//        }

//        public bool DeleteStudent(int studentId)
//        {
//            SqlParameter[] parameters = { new SqlParameter("@Stu_Id", studentId) };

//            int result = ExecuteNonQuery("spDeleteStudent", parameters);
//            return result > 0;
//        }
//    }
//}













using Crud_Pagging.Models;
using System.Data;
using System.Data.SqlClient;

namespace Crud_Pagging
{
    public class ApplicationDBContext
    {
        public static SqlConnection getcon()
        {

            string cs = "Data Source=SHAHM-LAP;Initial Catalog=Empl SP DB;Integrated Security=True";

            SqlConnection con = new SqlConnection(cs);
            return con;

        }


        //public List<StudentModel> GetAllStudents()
        //{
        //    List<StudentModel> listOfStudents = new List<StudentModel>();

        //    try
        //    {
        //        using (SqlConnection con = getcon())
        //        {
        //            string query = "SELECT * FROM students";

        //            SqlCommand cmd = new SqlCommand(query, con);
        //            con.Open();
        //            SqlDataReader dr = cmd.ExecuteReader();

        //            while (dr.Read())
        //            {
        //                listOfStudents.Add(new StudentModel()
        //                {
        //                    Stu_Id = Convert.ToInt32(dr["Stu_Id"]),
        //                    FirstName = dr["FirstName"].ToString(),
        //                    LastName = dr["LastName"].ToString(),
        //                    Age = Convert.ToInt32(dr["Age"]),
        //                    Gender = dr["Gender"].ToString()
        //                });
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it as required
        //        Console.WriteLine("An error occurred: " + ex.Message);
        //    }

        //    return listOfStudents;
        //}

        //using dataAdapter

        public List<StudentModel> GetAllStudents()
        {
            List<StudentModel> listOfStudents = new List<StudentModel>();

            try
            {
                using (SqlConnection con = getcon())
                {
                    string query = "SELECT * FROM students";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();

                    // Fill the DataTable with the result of the query
                    adapter.Fill(dt);

                    // Iterate through DataTable rows and populate the list of students
                    foreach (DataRow row in dt.Rows)
                    {
                        listOfStudents.Add(new StudentModel()
                        {
                            Stu_Id = Convert.ToInt32(row["Stu_Id"]),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Age = Convert.ToInt32(row["Age"]),
                            Gender = row["Gender"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return listOfStudents;
        }


        //public StudentModel GetStudentById(int id)
        //{
        //    StudentModel student = null;

        //    try
        //    {
        //        using (SqlConnection con = getcon())
        //        {
        //            string query = "SELECT * FROM students WHERE Stu_Id = @Stu_Id";

        //            SqlCommand cmd = new SqlCommand(query, con);
        //            cmd.Parameters.AddWithValue("@Stu_Id", id);

        //            con.Open();
        //            SqlDataReader dr = cmd.ExecuteReader();

        //            if (dr.Read())
        //            {
        //                student = new StudentModel()
        //                {
        //                    Stu_Id = Convert.ToInt32(dr["Stu_Id"]),
        //                    FirstName = dr["FirstName"].ToString(),
        //                    LastName = dr["LastName"].ToString(),
        //                    Age = Convert.ToInt32(dr["Age"]),
        //                    Gender = dr["Gender"].ToString()
        //                };
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it as required
        //        Console.WriteLine("An error occurred: " + ex.Message);
        //    }

        //    return student;
        //}

        // using datatable
        public StudentModel GetStudentById(int id)
        {
            StudentModel student = null;

            try
            {
                using (SqlConnection con = getcon())
                {
                    string query = "SELECT * FROM students WHERE Stu_Id = @Stu_Id";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@Stu_Id", id);

                    DataTable dt = new DataTable();

                    // Fill the DataTable with the result of the query
                    adapter.Fill(dt);

                    // Check if any row is returned
                    if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0];
                        student = new StudentModel()
                        {
                            Stu_Id = Convert.ToInt32(row["Stu_Id"]),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Age = Convert.ToInt32(row["Age"]),
                            Gender = row["Gender"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return student;
        }



        //Create new user
        //public bool createstudent(StudentModel student)
        //{
        //    // Using statement to ensure the connection is disposed properly
        //    using (SqlConnection con = getcon())
        //    {
        //        try
        //        {
        //            using (SqlCommand cmd = new SqlCommand("spcreatestudent", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
        //                cmd.Parameters.AddWithValue("@LastName", student.LastName);
        //                cmd.Parameters.AddWithValue("@Age", student.Age);
        //                cmd.Parameters.AddWithValue("@Gender", student.Gender);

        //                con.Open();
        //                int result = cmd.ExecuteNonQuery();

        //                // Check if the operation was successful (i.e., rows affected > 0)
        //                return result > 0;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log the exception or handle it as required
        //            Console.WriteLine("An error occurred: " + ex.Message);
        //            return false;
        //        }
        //    }
        //}

        public bool createstudent(StudentModel student)
        {
            using (SqlConnection con = getcon())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("spcreatestudent", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = student.FirstName;
                        cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = student.LastName;
                        cmd.Parameters.Add("@Age", SqlDbType.Int).Value = student.Age;
                        cmd.Parameters.Add("@Gender", SqlDbType.Char).Value = student.Gender;

                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            adapter.InsertCommand = cmd;
                            con.Open();

                            // Execute the InsertCommand using the DataAdapter
                            int result = adapter.InsertCommand.ExecuteNonQuery();

                            // Check if the operation was successful (i.e., rows affected > 0)
                            return result > 0;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Log the SqlException error or handle it as required
                    Console.WriteLine("SQL error occurred: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    // Log the general exception or handle it as required
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return false;
                }
            }
        }



        // update student 

        public bool UpdateStudent(StudentModel stu)
        {
            using (SqlConnection con = getcon())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spupdatestudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Stu_Id", stu.Stu_Id); // Add ID parameter
                    cmd.Parameters.AddWithValue("@FirstName", stu.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", stu.LastName);
                    cmd.Parameters.AddWithValue("@Age", stu.Age);
                    cmd.Parameters.AddWithValue("@Gender", stu.Gender);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    return i > 0;
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as required
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return false; // Return false if an exception occurs
                }
            }
        }


        //delete Data

        public bool DeleteStudent(int studentId)
        {
            try
            {
                using (SqlConnection con = getcon())
                {
                    SqlCommand cmd = new SqlCommand("spDeleteStudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Stu_Id", studentId);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();


                    return rowsAffected > 0; // Return true if at least one row was deleted
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred: " + ex.Message); // Use your preferred logging framework
                return false; // Return false if an exception occurs
            }
        }

    }



}

