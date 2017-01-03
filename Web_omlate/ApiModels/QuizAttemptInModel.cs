using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_omlate.ApiModels
{
    public class QuizAttemptInModel
    {
        public String QuizId { get; set; }
        public String LearnerId { get; set; }
        public String Answers { get; set; }
    }
}