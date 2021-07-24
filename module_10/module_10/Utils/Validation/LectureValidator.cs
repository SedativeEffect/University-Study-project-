using System;
using FluentValidation;
using module_10.Models;

namespace module_10.Utils.Validation
{
    public class LectureValidator : AbstractValidator<LectureInput>
    {
        public LectureValidator()
        {
            RuleFor(x => x.Subject).InclusiveBetween(0, 3);
            RuleFor(x => x.LectureDate).GreaterThan(DateTime.Now.AddDays(3));
        }
    }
}
