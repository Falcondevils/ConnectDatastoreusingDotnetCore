using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectDatastoreusingDotnetCore.Models
{
    public class Question
    {
        public string Author { get; set; }
        public string Quiz { get; set; }
        public string Title { get; set; }
        public string Answer1  { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
