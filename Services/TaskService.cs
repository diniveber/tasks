using Tasks.Interfaces;
using Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;



namespace Tasks.Controllers
{   
    public class TaskService : ITaskHttp
    {    
        private List<Task> Tasks{ get; }

        private IWebHostEnvironment  webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Tasks.json");
            //this.filePath = webHost.ContentRootPath+@"/Data/Pizza.json";
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }

        public List<Task> GetAll() => tasks;
        public Task Get(int id)
        {
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Task task)
        {
            task.Id = tasks.Max(t => t.Id) + 1;
            tasks.Add(task);
            saveToFile();

        }

        public bool Update(int id, Task newTask)
        {
            if (newTask.Id != id)
                return false;
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;
            task.Name = newTask.Name;
            task.Done = newTask.Done;
            saveToFile();
            return true;
        }

        public bool Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;
            tasks.Remove(task);
            saveToFile();
            return true;
        }
    }
}