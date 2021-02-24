using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionCategory
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int QuestionId { get; set; }

        public static List<QuestionCategory> toListCategory(List<string> pList)
        {
            if (pList == null)
                return null;
            List<QuestionCategory> list = new List<QuestionCategory>();

            pList.ForEach(c => list.Add(new QuestionCategory() { Category = c }));

            return list;
        }
        public static List<string> toListString(List<QuestionCategory> pList)
        {
            if (pList == null)
                return null;
            List<string> list = new List<string>();

            pList.ForEach(c => list.Add(c.Category));

            return list;
        }
    }
}