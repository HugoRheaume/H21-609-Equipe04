using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Validation
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class QuestionValidationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                SetBadRequest(actionContext);
            }

            QuestionCreateDTO q = (QuestionCreateDTO)actionContext.ActionArguments.Values.First();
            if(q.QuestionType == 0 || q.Label == "" || q.Label == null)
            {
                SetBadRequest(actionContext);
            }
            if(q.Label.Length > 250)
            {
                SetBadRequest(actionContext);
            }

            switch (q.QuestionType)
            {
                case QuestionType.TrueFalse:
                    if (q.QuestionTrueFalse == null) SetBadRequest(actionContext);                    
                    break;
                case QuestionType.MultipleChoice:
                    if (q.QuestionMultipleChoice == null) SetBadRequest(actionContext);
                    break;
                case QuestionType.Association:
                    if (q.QuestionAssociation == null) SetBadRequest(actionContext);
                    break;
                default:
                    break;
            }
        }

        private void SetBadRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

    }
}