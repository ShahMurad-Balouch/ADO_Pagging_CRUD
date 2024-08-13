using Crud_Pagging;
using Crud_Pagging.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CrudWithDataTablesUsingAdo.net.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        string query;
        public SqlDataAdapter adapter;
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Indexx()
        {
            try
            {
                var start = Convert.ToInt32(Request.Form["start"]);
                var pageSize = Convert.ToInt32(Request.Form["length"]);
                var pageNumber = start / pageSize + 1;
                string searchKeyword = Request.Form["search[value]"];
                string sortDirection = Request.Form["order[0][dir]"];
                string sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];

                using (SqlConnection con = ApplicationDBContext.getcon())
                {
                    SqlCommand cmd = new SqlCommand("spgetstu", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@SearchKeyword", string.IsNullOrEmpty(searchKeyword) ? DBNull.Value : (object)searchKeyword);
                    cmd.Parameters.AddWithValue("@sortColumn", sortColumn);
                    cmd.Parameters.AddWithValue("@sortDirection", sortDirection);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Fetch paginated data
                    List<StudentModel> data = new List<StudentModel>();
                    int totalRecords = 0;
                    int filteredRecords = 0;

                    while (reader.Read())
                    {
                        data.Add(new StudentModel
                        {
                            Stu_Id = Convert.ToInt32(reader["Stu_Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Gender = reader["Gender"].ToString()
                        });
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        totalRecords = Convert.ToInt32(reader["TotalRecords"]);
                        filteredRecords = Convert.ToInt32(reader["FilteredRecords"]);
                    }

                    return Json(new
                    {
                        draw = Convert.ToInt32(Request.Form["draw"]),
                        recordsTotal = totalRecords,
                        recordsFiltered = filteredRecords,
                        data = data
                    });
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { success = false, message = ex.Message });
            }
        
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentModel stu)
        {
            try
            {

                if (ModelState.IsValid == true)
                {
                    ApplicationDBContext context = new ApplicationDBContext();
                    bool check = context.createstudent(stu);
                    if (check == true)
                    {
                        TempData["InsertedMessage"] = "Data is inserted";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                }
                return View();

            }
            catch
            {
                return View();
            }
        }



        public IActionResult Edit(int id) {
            ApplicationDBContext context = new ApplicationDBContext();
            var edited = context.GetStudentById(id);
            return View(edited);
        }
        [HttpPost]
        public IActionResult Edit(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                ApplicationDBContext context = new ApplicationDBContext();

                bool isUpdated = context.UpdateStudent(student);

                if (isUpdated)
                {
                    TempData["UpdatedMessage"] = "Student updated successfully.";
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update student.";
                }
            }

            // If we reach here, something went wrong, so return the view with the model
            return View(student);
        }

       
        public IActionResult Delete(int id)
        {
            ApplicationDBContext context = new ApplicationDBContext();
            var student = context.GetStudentById(id);
            if (student == null)
            {
                return NotFound(); // Return a 404 error if the student is not found
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            ApplicationDBContext context = new ApplicationDBContext();
            bool isDeleted = context.DeleteStudent(id);

            if (isDeleted)
            {
                TempData["DeletedMessage"] = "Student deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete student.";
            }

            return RedirectToAction("Index");
        }
    
    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}