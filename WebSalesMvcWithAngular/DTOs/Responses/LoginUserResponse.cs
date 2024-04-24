using System.Text.Json.Serialization;

namespace WebSalesMvcWithAngular.DTOs.Responses
{
    public class LoginUserResponse
    {
        public bool Success {  get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull )]
        public string? Token { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ExpireDate { get; private set; }    
        public List<string> Errors { get; private set; }
        public LoginUserResponse() =>
            Errors = new List<string>();

        public LoginUserResponse(bool success = true, string? token = null, DateTime? expireDate = null) : this() =>
            Success = success;

         public void AddError(string error) =>
            Errors.Add(error);

        public void AddErrors(IEnumerable<string> errors) =>
          Errors.AddRange(errors);

        public LoginUserResponse(bool success, string token, DateTime expireDate, List<string>? errors) : this(success)
        {
            Token = token;
            ExpireDate = expireDate;
            Errors = errors;
        }

        public LoginUserResponse(bool success, string token, DateTime expireDate) : this(success)
        {
            Token = token;
            ExpireDate = expireDate;
        }
    }
}
