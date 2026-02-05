using System.Text.Json.Serialization;

namespace Sky.Api.Application.Responses
{
    public class Response<TData>
    {
        private readonly int _code;

        [JsonConstructor]
        public Response()
            => _code = 200;

        public Response(TData? data, int code = 200, string? message = null)
        {
            _code = code;
            Data = data;
            Message = message;
        }
        public int Code => _code;
        public TData? Data { get; set; }
        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess => _code is >= 200 and <= 299;
    }
}
