using System.Transactions;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;

namespace AspNetRazorTest
{
    [TestClass]
    public class WorkTaskTest
    {

        [DataTestMethod]
        [DataRow("NewTask", 1, 1)]
        public void Insert_NewTask(string name, int priority, int statusId)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                WorkTask newTask = new WorkTask();

                newTask.Name = name;
                newTask.Priority = priority;
                newTask.StatusId = statusId;

                int rowCountStart = WorkTaskDbHelper.WorkTasks.Count;

                WorkTaskDbHelper.InsertWorkTask(newTask);

                int rowCountEnd = WorkTaskDbHelper.WorkTasks.Count;

                Assert.AreEqual(rowCountStart + 1, rowCountEnd);
            }
        }

        [DataTestMethod]
        [DataRow("NewTaskDelete", 1, 3)]
        public void Delete_WorkTask(string name, int priority, int statusId)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                WorkTask newTask = new WorkTask();

                newTask.Name = name;
                newTask.Priority = priority;
                newTask.StatusId = statusId;

                int rowCountStart = WorkTaskDbHelper.WorkTasks.Count;

                WorkTaskDbHelper.InsertWorkTask(newTask);

                int insertedId = WorkTaskDbHelper.WorkTasks.FirstOrDefault(x => x.Name == name).Id;

                WorkTaskDbHelper.DeleteWorkTask(insertedId);
                
                int rowCountEnd = WorkTaskDbHelper.WorkTasks.Count;

                Assert.AreEqual(rowCountStart, rowCountEnd);
            }
        }

        [DataTestMethod]
        [DataRow("NewTaskForUpdate", 1, 1)]
        public void Update_WorkTask(string name, int priority, int statusId)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                WorkTask newTask = new WorkTask();

                newTask.Name = name;
                newTask.Priority = priority;
                newTask.StatusId = statusId;

                int rowCountStart = WorkTaskDbHelper.WorkTasks.Count;

                WorkTaskDbHelper.InsertWorkTask(newTask);

                newTask.Id = WorkTaskDbHelper.WorkTasks.FirstOrDefault(x => x.Name == name).Id;

                string oldName = newTask.Name;

                newTask.Name = "Changed Name";

                WorkTaskDbHelper.UpdateWorkTask(newTask);

                string changedName = WorkTaskDbHelper.WorkTasks.FirstOrDefault(x => x.Id == newTask.Id).Name;

                Assert.AreNotEqual(oldName.Trim(), changedName.Trim());
            }
        }
    }
}