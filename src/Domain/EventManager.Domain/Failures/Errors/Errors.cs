using EventsManager.Failures.Errors;
using System.Text.Json;

namespace EventManager.Domain.Failures.Errors
{
    public class ErrorsCollection : List<Error>
    {
        public string Json => JsonSerializer.Serialize(this);

        public bool HasErrors => this.Count > 0;

        public ErrorsCollection(IEnumerable<Error> errors)
        {
            AddRange(errors);
        }

        public ErrorsCollection()
        {
            
        }
    }
}
