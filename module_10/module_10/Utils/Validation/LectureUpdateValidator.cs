using System;
using FluentValidation;
using module_10.Models;

namespace module_10.Utils.Validation
{
    public class LectureUpdateValidator : AbstractValidator<LectureToUpdate>
    {
        public LectureUpdateValidator()
        {
            RuleFor(x => x.Subject).InclusiveBetween(0, 3);
            RuleFor(x => x.LectureDate).GreaterThan(DateTime.Now.AddDays(3));
        }
    }
}
