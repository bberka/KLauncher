using System.ComponentModel.DataAnnotations;

namespace KLauncher.Shared.Validators;

public class FilePathValidatorAttribute : ValidationAttribute
{
  protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
    var isNullable = Nullable.GetUnderlyingType(validationContext.ObjectType) != null;
    if (isNullable && value is null) return ValidationResult.Success!;
    var isString = value is string;
    if (!isString) {
      var isStringArrayOrList = value is IEnumerable<string>;
      if (isStringArrayOrList) {
        var stringArray = value as IEnumerable<string>;
        foreach (var filePath in stringArray) {
          if (string.IsNullOrWhiteSpace(filePath)) return new ValidationResult($"{validationContext.DisplayName} is null or empty");
          if (filePath.StartsWith("{") && filePath.EndsWith("}")) continue;
          if (!File.Exists(filePath)) return new ValidationResult($"{validationContext.DisplayName} does not exists");
        }

        return ValidationResult.Success!;
      }

      return new ValidationResult($"{validationContext.DisplayName} is not a string or string array");
    }

    var path = value as string;
    if (string.IsNullOrWhiteSpace(path)) return new ValidationResult($"{validationContext.DisplayName} is null or empty");
    if (!File.Exists(path)) return new ValidationResult($"{validationContext.DisplayName} does not exists");
    return ValidationResult.Success!;
  }
}