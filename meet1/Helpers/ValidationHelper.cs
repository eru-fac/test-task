using System.ComponentModel.DataAnnotations;

namespace MeetingsApi.Helpers;

public static class ValidationHelper
{
    public static Dictionary<string, string[]> Validate(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model);

        Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true);

        return validationResults
            .GroupBy(x => x.MemberNames.FirstOrDefault() ?? string.Empty)
            .ToDictionary(
                x => x.Key,
                x => x.Select(result => result.ErrorMessage ?? "Validation error").ToArray()
            );
    }

    public static void AddDateValidation(
        Dictionary<string, string[]> errors,
        DateTime startTime,
        DateTime endTime)
    {
        if (endTime <= startTime)
        {
            errors["EndTime"] = new[] { "EndTime must be later than StartTime." };
        }
    }

    public static bool HasErrors(Dictionary<string, string[]> errors)
    {
        return errors.Count > 0;
    }
}
