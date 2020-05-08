using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConnectDatastoreusingDotnetCore.Models;
using Google.Cloud.Datastore.V1;

namespace ConnectDatastoreusingDotnetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // Your Google Cloud Platform project IDcmd
        private readonly string projectId = "qualified-ace-275901";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateQuestion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateQuestion(Question ques)
        {
         

            // Instantiates a client
            DatastoreDb db = DatastoreDb.Create(projectId);

        // The kind for the new entity
            string kind = "Questions";
            KeyFactory keyFactory = db.CreateKeyFactory(kind);
            // The Cloud Datastore key for the new entity
            //Key key = keyFactory.CreateKey(name);

            var task = new Entity
            {
                Key = keyFactory.CreateIncompleteKey(),
                ["Author"] = ques.Author,
                ["Quiz"] = ques.Quiz,
                ["Title"] =ques.Title,
                ["Answer1"]=ques.Answer1,
                ["Answer2"] =ques.Answer2,
                ["Answer3"] =ques.Answer3,
                ["Answer4"] =ques.Answer4,
                ["CorrectAnswer"] =ques.CorrectAnswer
            };
            using (DatastoreTransaction transaction = db.BeginTransaction())
            {
                // Saves the task
                task.Key = db.Insert(task);
                //transaction.Upsert(task);
                transaction.Commit();

                Console.WriteLine($"Saved {task.Key.Path[0].Name}: {(string)task["Quiz"]}");
            }

            return RedirectToAction("QuestionList", "Home");
        }
        [HttpGet]
        public IActionResult QuestionList()
        {
            // Instantiates a client
            DatastoreDb db = DatastoreDb.Create(projectId);
            List<Question> questions = new List<Question>();
            // The kind for the new entity
            string kind = "Questions";
            KeyFactory keyFactory = db.CreateKeyFactory(kind);
            Query query = new Query("Questions");
            //{
            //    Projection = { "Author" }//, "Quiz", "Title", "Answer1", "Answer2", "Answer3" , "Answer4", "CorrectAnswer"}
            //};
            //{
            //    Filter = Filter.And(Filter.Equal("done", false),
            //    Filter.GreaterThanOrEqual("priority", 4)),
            //    Order = { { "priority", PropertyOrder.Types.Direction.Descending } }
            //};
            //DatastoreQueryResults tasks = db.RunQuery(query);
            foreach (var entity in db.RunQueryLazily(query))
            {
                Question ques = new Question
                {

                    Author = entity["Author"].StringValue,
                    Quiz = entity["Quiz"].StringValue,
                    Title = entity["Title"].StringValue,
                    Answer1 = entity["Answer1"].StringValue,
                    Answer2 = entity["Answer2"].StringValue,
                    Answer3 = entity["Answer3"].StringValue,
                    Answer4 = entity["Answer4"].StringValue,
                    CorrectAnswer = entity["CorrectAnswer"].StringValue

                };
                questions.Add(ques);
            }
            return View(questions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
