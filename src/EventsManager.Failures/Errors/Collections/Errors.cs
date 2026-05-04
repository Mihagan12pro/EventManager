using System.Text.Json;

namespace EventsManager.Failures.Errors.Collections
{
    public class ErrorsCollection : List<Error>
    {
        public string Json => JsonSerializer.Serialize(this);

        public ErrorsCollection(IEnumerable<Error> errors)
        {
            AddRange(errors);
        }

        public ErrorsCollection()
        {
            
        }
    }
}
