using System;
using System.Collections.Generic;

namespace CRUDApp.Model
{
    public class MockHomeworkRepository : IHomeworkRepository
    {
        public IList<Homework> GetHomework()
        {
            return new[] 
            {
                new Homework() { Course = "Vak 1", DueDate = DateTime.Now.AddDays(5), Summary = "Een kleine uitleg" },
                new Homework() { Course = "Vak 1", DueDate = DateTime.Now.AddDays(6), Summary = "Een kleine uitleg", Completed =  true },
                new Homework() { Course = "Vak 2", DueDate = DateTime.Now.AddDays(15), Summary = "Een kleine uitleg" },
                new Homework() { Course = "Vak 2", DueDate = DateTime.Now.AddDays(1), Summary = "Een kleine uitleg" },
                new Homework() { Course = "Vak 3", DueDate = DateTime.Now.AddDays(22), Summary = "Een kleine uitleg" }
            };
        }
    }
}
