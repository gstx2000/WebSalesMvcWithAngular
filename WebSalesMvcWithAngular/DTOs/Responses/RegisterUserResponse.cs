
namespace WebSalesMvcWithAngular.DTOs.Responses
{
    public class RegisterUserResponse
    {
        public bool Success { get; private set; }
        public HashSet<string> Errors { get; private set; }

        public RegisterUserResponse()
        {
            Errors = new HashSet<string>();
        }

        public RegisterUserResponse(bool success = true) : this()
        {
            Success = success;
        }

        public void AddErrors(IEnumerable<string> errors) => Errors.UnionWith(errors);

        public void AddError(string error) => Errors.Add(error);
    }
}
