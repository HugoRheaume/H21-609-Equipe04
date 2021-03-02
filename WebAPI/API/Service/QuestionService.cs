using API.Models;
using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net.Http.Headers;

namespace API.Service
{
    public class QuestionService : BaseService, IQuestionService
    {

        public QuestionService(ApplicationDbContext db) : base(db) { }

        public QuestionService() { }

        public List<QuestionDTO> GetQuestions()
        {

            List<QuestionDTO> listToShip = new List<QuestionDTO>();

            List<Question> questionList = db.Question.Include(x => x.Quiz).ToList();
            foreach (var item in questionList)
            {
                listToShip.Add(GenerateQuestionDTO(item));
            }

            return listToShip;
        }
        public QuestionDTO GetQuestionById(int id)
        {
            Question question = db.Question.Include(q => q.Quiz).FirstOrDefault(x => x.Id == id);
            if (question != null)
                return GenerateQuestionDTO(question);

            return null;
        }

        public List<QuestionDTO> GetQuestionsByShareCode(string shareCode)
        {
            Quiz quiz = db.ListQuiz.FirstOrDefault(x => x.ShareCode == shareCode);
            if (quiz == null)
                return null;

            List<Question> questionList = quiz.ListQuestions;

            return questionList.Select(item => GenerateQuestionDTO(item)).OrderBy(x => x.QuizIndex).ToList();
        }

        public QuestionDTO AddQuestion(QuestionCreateDTO question)
        {

            Quiz quiz = db.ListQuiz.FirstOrDefault(x => x.Id == question.QuizId);

            Question questionToCreate = new Question()
            {
                Quiz = quiz,
                Label = question.Label,
                TimeLimit = question.TimeLimit,
                QuestionType = question.QuestionType,
                QuestionTrueFalse = new List<QuestionTrueFalse>() { question.QuestionTrueFalse },
                QuestionMultipleChoice = question.QuestionMultipleChoice,
                NeedsAllAnswers = question.NeedsAllAnswers,
                QuestionAssociation = question.QuestionAssociation,
                Categories = QuestionCategory.toListCategory(question.Categories)
            };

            Question q = db.Question.Add(questionToCreate);
            db.SaveChanges();
            return GenerateQuestionDTO(q);
        }


        private QuestionDTO GenerateQuestionDTO(Question q)
        {
            QuestionDTO question = new QuestionDTO()
            {
                Id = q.Id,
                QuizId = q.Quiz.Id,
                Label = q.Label,
                TimeLimit = q.TimeLimit,
                QuestionType = q.QuestionType,
                QuizIndex = q.QuizIndex,
                NeedsAllAnswers = q.NeedsAllAnswers,
                Categories = QuestionCategory.toListString(q.Categories)


            };
            switch (q.QuestionType)
            {
                case QuestionType.TrueFalse:
                    question.QuestionTrueFalse = q.QuestionTrueFalse.FirstOrDefault();
                    break;
                case QuestionType.MultipleChoice:
                    question.QuestionMultipleChoice = q.QuestionMultipleChoice;
                    break;
                case QuestionType.Association:
                    question.QuestionAssociation = q.QuestionAssociation;
                    break;
                case QuestionType.Image:
                    break;
                default:
                    break;
            }

            return question;
        }

        public List<QuestionDTO> DeleteQuestion(int id)
        {
            Question question = db.Question.Where(x => x.Id == id).Include(x => x.Quiz).FirstOrDefault();
            if (question == null)
                return null;

            int quizId = question.Quiz.Id;

            db.Question.Remove(question);
            db.SaveChanges();
            return GetQuestionByQuizId(quizId);

        }
        public bool UpdateQuizIndex(List<QuestionDTO> questions)
        {
            foreach (var item in questions)
            {
                Question q = db.Question.Include(x => x.Quiz).FirstOrDefault(x => x.Id == item.Id);
                if (q == null)
                    continue;
                q.QuizIndex = item.QuizIndex;
            }
            db.SaveChanges();
            return true;

        }


        /// <summary>
        /// Obtient la liste des questions pour un Quiz à partir de l'Id.
        /// </summary>
        /// <param name="quizId">Id du quiz dont on veut les questions</param>
        /// <returns>List<QuestionDTO></returns>
        public virtual List<QuestionDTO> GetQuestionByQuizId(int quizId)
        {
            Quiz quiz = db.ListQuiz.FirstOrDefault(x => x.Id == quizId);
            if (quiz == null)
                return null;
            List<Question> questionList = db.Question.Where(x => x.Quiz.Id == quizId).ToList();

            return questionList.Select(item => GenerateQuestionDTO(item)).OrderBy(x => x.QuizIndex).ToList();
        }


        /// <summary>
        /// Renvoie la première question d'un Quiz si le questionId est -1.
        /// Sinon renvoie la question suivante.
        /// </summary>
        /// <param name="quizId">Id du quiz</param>
        /// <param name="questionId">Id de la question</param>
        /// <returns>QuestionDTO</returns>
        public QuestionDTO GetNextQuestion(int quizId, int questionId)
        {
            List<QuestionDTO> list = GetQuestionByQuizId(quizId);
            Question question = GetDBQuestionById(questionId);
            int index = -1;
            if (questionId > -1)
                index = question.QuizIndex;
            if (list.Count <= index + 1)
                return new QuestionDTO()
                {
                    Id = 0,
                    QuizId = quizId,
                    Label = "",
                    TimeLimit = 0,
                    QuestionType = QuestionType.TrueFalse,
                    QuizIndex = -1,
                    NeedsAllAnswers = false,
                };
            return list[index + 1];
        }

        public virtual Question GetDBQuestionById(int questionId)
        {
            return db.Question.Find(questionId);
        }
        
        /// <summary>
        /// Sauvegarde un résultat à une question associé à un User dans la DB
        /// </summary>
        /// <param name="result">Résultat</param>
        /// <param name="cookie">Token du User</param>
        public void StoreQuestionResult(QuestionResultDTO result, string token)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            db.QuestionResult.Add(new QuestionResult()
            {
                Question = db.Question.Find(result.QuestionId),
                User = user,
                Score = result.Score
            });

            db.SaveChanges();
        }


        public bool ModifyQuesiton(QuestionDTO modifiedDTO)
        {
            Question questionToModify = db.Question.FirstOrDefault(q => q.Id == modifiedDTO.Id);
            if (questionToModify != null)
            {
                //Modifications communes a toutes les questions
                questionToModify.Label = modifiedDTO.Label;
                questionToModify.TimeLimit = modifiedDTO.TimeLimit;

                //Modifications uniques a certains types de questions
                switch (questionToModify.QuestionType)
                {
                    case QuestionType.TrueFalse:
                        {
                            QuestionTrueFalse question = db.QuestionTrueFalse.FirstOrDefault(q => q.QuestionId == questionToModify.Id);
                            question.Answer = modifiedDTO.QuestionTrueFalse.Answer;
                            break;
                        }
                    case QuestionType.MultipleChoice:
                        {
                            List<QuestionMultipleChoice> choicesInDB = db.QuestionMultiple.Where(q => q.QuestionId == questionToModify.Id).ToList();
                            List<QuestionMultipleChoice> newChoices = modifiedDTO.QuestionMultipleChoice;

                            for (int i = 0; i < newChoices.Count; i++)
                            {
                                QuestionMultipleChoice newChoice = newChoices[i];
                                QuestionMultipleChoice oldChoice = choicesInDB.FirstOrDefault(c => c.Id == newChoice.Id);
                                if (oldChoice == null) // Crée le nouveau choix dans la BD
                                {
                                    newChoice.QuestionId = questionToModify.Id;
                                    db.QuestionMultiple.Add(newChoice);
                                    continue;
                                }
                                // Modifie l'ancien choix dans la BD
                                oldChoice.Answer = newChoice.Answer;
                                oldChoice.Statement = newChoice.Statement;
                                choicesInDB.Remove(oldChoice);
                            }

                            // Si le choix a été enlevé au cour de la modification.
                            if (choicesInDB.Count > 0)
                            {
                                foreach (QuestionMultipleChoice choice in choicesInDB)
                                {
                                    db.QuestionMultiple.Remove(choice);
                                }
                            }

                            questionToModify.NeedsAllAnswers = modifiedDTO.NeedsAllAnswers;
                            break;
                        }
                    case QuestionType.Association:
                        {
                            List<QuestionAssociation> associationsInDB = questionToModify.QuestionAssociation.ToList();
                            List<QuestionCategory> categoriesInDB = questionToModify.Categories.ToList();

                            List<QuestionAssociation> newAssociations = modifiedDTO.QuestionAssociation;
                            List<QuestionCategory> newCategories = QuestionCategory.toListCategory(modifiedDTO.Categories);

                            for (int i = 0; i < newCategories.Count; i++)
                            {
                                QuestionCategory newCat = newCategories[i];
                                QuestionCategory oldCat = categoriesInDB.Count > 0 ? categoriesInDB[0] : null;

                                // Si la categorie est nouvelle
                                if (oldCat == null)
                                {
                                    questionToModify.Categories.Add(new QuestionCategory() { Category = newCat.Category });
                                    db.SaveChanges();
                                }
                                // Si la categorie est modifiee
                                questionToModify.Categories[i].Category = newCat.Category;
                                categoriesInDB.Remove(oldCat);
                            }
                            // Si une categorie a ete supprimee
                            if (categoriesInDB.Count > 0)
                            {
                                foreach (QuestionCategory category in categoriesInDB)
                                {
                                    db.QuestionCategory.Remove(category);
                                }
                            }

                            for (int i = 0; i < newAssociations.Count; i++)
                            {
                                QuestionAssociation newAsso = newAssociations[i];
                                QuestionAssociation oldAsso = associationsInDB.Count > 0 ? associationsInDB[0] : null;

                                // Si l'association est nouvelle
                                if (oldAsso == null)
                                {
                                    questionToModify.QuestionAssociation.Add(new QuestionAssociation()
                                    {
                                        Statement = newAsso.Statement,
                                        CategoryIndex = newAsso.CategoryIndex
                                    });
                                    db.SaveChanges();
                                }
                                // Si l'association est modifiee
                                questionToModify.QuestionAssociation[i].CategoryIndex = newAsso.CategoryIndex;
                                questionToModify.QuestionAssociation[i].Statement = newAsso.Statement;
                                associationsInDB.Remove(oldAsso);
                            }
                            // Si une association est supprimee
                            if (associationsInDB.Count > 0)
                            {
                                foreach (QuestionAssociation association in associationsInDB)
                                {
                                    db.QuestionAssociation.Remove(association);
                                }
                            }

                            break;
                        }
                }

                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}