using DiaryScheduler.Web.Common.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DiaryScheduler.Web.Common.Classes
{
    public class SiteErrorHandler
    {
        public static ActionResult GetBadRequestActionResult(SiteValidationException ex)
        {
            return GetBadRequestActionResult(ex.ValidationErrors);
        }

        public static ActionResult GetBadRequestActionResult(string errormessage, string propertyName)
        {
            return GetBadRequestActionResult(errormessage, new string[] { propertyName });
        }

        public static ActionResult GetBadRequestActionResult(string errormessage, string[] propertyName)
        {
            return GetBadRequestActionResult(new List<ValidationResult>() { new ValidationResult(errormessage, propertyName) });
        }

        public static ActionResult GetBadRequestActionResult(IEnumerable<ValidationResult> validationResults)
        {
            return new JsonContentResult(JsonConvert.SerializeObject(validationResults), HttpStatusCode.BadRequest);
        }

        public static ActionResult GetBadRequestActionResult(Dictionary<string, IEnumerable<string>> errors)
        {
            List<ValidationResult> vr = GetValidationResults(errors);
            return GetBadRequestActionResult(vr);
        }

        private static List<ValidationResult> GetValidationResults(Dictionary<string, IEnumerable<string>> errors)
        {
            List<ValidationResult> vr = new List<ValidationResult>();

            foreach (var kvp in errors)
            {
                vr.Add(new ValidationResult(kvp.Value.FirstOrDefault(), new string[] { kvp.Key }));
            }

            return vr;
        }

        internal static ActionResult GetBadRequestActionResult(ModelStateDictionary modelState)
        {
            var errors = modelState.Where(x => x.Value.Errors.Any()).ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage));
            return GetBadRequestActionResult(errors);
        }

        internal static SiteValidationException GetValidationException(ModelStateDictionary modelState)
        {
            var errors = modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage));
            return new SiteValidationException(GetValidationResults(errors));
        }
    }
}