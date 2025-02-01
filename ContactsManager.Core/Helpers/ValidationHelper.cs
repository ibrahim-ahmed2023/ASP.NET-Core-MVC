using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResult.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
