using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Shared.Failures.Errors
{
    public class ErrorsCollection : List<Error>
    {
        public string Json => JsonSerializer.Serialize(this);

        public bool HasErrors => this.Count > 0;

        public void AddRange(params List<ErrorsCollection> errorsCollections)
        {
            foreach (var errors in errorsCollections)
            {
                foreach (var error in errors)
                {
                    Add(error);
                }
            }
        }

        public ErrorsCollection(IEnumerable<Error> errors)
        {
            AddRange(errors);
        }

        public ErrorsCollection(params IEnumerable<ValidationResult> validationResults)
        {
            foreach (var validationResult in validationResults)
                Add(new Error(validationResult.ErrorMessage));
        }

        public ErrorsCollection()
        {

        }
    }
}
