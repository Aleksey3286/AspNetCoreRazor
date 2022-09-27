using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace AspNetRazor.Models
{
    public static class WorkTaskDbHelper
    {
        private static string ConnectionString = @"Data Source=ALEKSEY-AN\MSSQLSERVER01;Initial Catalog=TasksDB;Integrated Security=True";

        public static SqlConnection Connection { get; set; }

        private static List<WorkTask> workTasks;

        private static List<Status> statuses;


        public static List<WorkTask> WorkTasks
        {
            get
            {
                if (workTasks is null)
                {
                    InitializeDB();
                }
                
                return workTasks;
            }
        }

        public static List<Status> Statuses
        {
            get
            {
                if (statuses is null)
                {
                    InitializeDB();
                }

                return statuses;
            }
        }

        private static void InitializeDB()
        {
            try
            {
                ReadStatuses();
                ReadWorkTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public static void ReadStatuses()
        {
            statuses = new List<Status>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = "SELECT Id, Name FROM [dbo].[Statuses]";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            statuses.Add(new Status(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToString(reader["Name"]).Trim()
                                ));
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
        }
        
        public static void ReadWorkTasks()
        {
            workTasks = new List<WorkTask>();
            
            if (statuses is null)
                ReadStatuses();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT Id, Name, Priority, StatusId FROM dbo.Tasks";

                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    workTasks = new List<WorkTask>();
                    int statusId = 0;


                    while (reader.Read())
                    {
                        statusId = Convert.ToInt32(reader["StatusId"]);

                        workTasks.Add(
                            new WorkTask(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToString(reader["Name"]).Trim(),
                                Convert.ToInt16(reader["Priority"]),
                                Convert.ToInt32(reader["StatusId"]),
                                statuses.FirstOrDefault(x => x.Id == statusId).Name
                                )
                            );
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void InsertWorkTask(WorkTask task)
        {
            TaskNameExist(task);

            int insertedTaskId = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("TaskInsert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", task.Name);
                    cmd.Parameters.AddWithValue("@Priority", task.Priority);
                    cmd.Parameters.AddWithValue("@StatusId", task.StatusId);

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        Direction = ParameterDirection.Output,
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int
                    });

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    insertedTaskId = cmd.Parameters["@Id"].Value is not null
                        ? Convert.ToInt32(cmd.Parameters["@Id"].Value)
                        : 0;
                }
            }

            if (insertedTaskId > 0)
            {
                task.Id = insertedTaskId;
                workTasks.Add(task);
            }
        }

        public static void DeleteWorkTask(int Id)
        {
            if (WorkTasks.FirstOrDefault(x=>x.Id == Id).StatusId 
                != Statuses.FirstOrDefault(x=>x.Name == "completed").Id)
            {
                throw new Exception("You are not able to delete uncompleted task");
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("TaskDelete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", Id);                    

                    connection.Open();

                    int rowAffected = cmd.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        workTasks.Remove(workTasks.FirstOrDefault(x => x.Id == Id));
                    }
                }
            }
        }

        public static void UpdateWorkTask(WorkTask task)
        {
            TaskNameExist(task);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("TaskUpdate", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", task.Id);
                    cmd.Parameters.AddWithValue("@Name", task.Name);
                    cmd.Parameters.AddWithValue("@Priority", task.Priority);
                    cmd.Parameters.AddWithValue("@StatusId", task.StatusId);

                    connection.Open();

                    int rowAffected = cmd.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        int indexOfTask = workTasks.FindIndex(x => x.Id == task.Id);
                        workTasks[indexOfTask].Name = task.Name;
                        workTasks[indexOfTask].Priority = task.Priority;
                        workTasks[indexOfTask].StatusId = task.StatusId;
                        workTasks[indexOfTask].StatusName = task.StatusName;
                    }
                }
            }           
        }

        private static void TaskNameExist(WorkTask task)
        {
            if (workTasks.Exists(x=>x.Name.TrimEnd() == task.Name.TrimEnd() && x.Id != task.Id))
                throw new DataException("Can't insert new tsak. Task with this name is exist.");
        }
    }
}
