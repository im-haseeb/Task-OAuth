namespace OAuth.Responses
{
	public class CustomResponse
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }

		public CustomResponse(int statusCode, string message, object? data = null)
		{
			StatusCode = statusCode;
			Message = message;
			Data = data;
		}
	}

}
