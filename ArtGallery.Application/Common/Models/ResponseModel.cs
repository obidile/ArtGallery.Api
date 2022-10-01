namespace ArtGallery.Application.Common.Models;
public class ResponseModel
{
    public ResponseModel()
    {

    }

    internal ResponseModel(bool succeeded, string message, int code, IDictionary<string, string[]> errors = null)
    {
        Status = succeeded;
        Message = message;
        Code = code;
        Errors = errors; // ?? new Dictionary<string, string[]>();
    }

    public bool Status { get; set; }

    public string Message { get; set; }

    public int Code { get; set; }

    public IDictionary<string, string[]> Errors { get; set; }

    public static ResponseModel Success(string message = "Request was Successful")
    {
        return new ResponseModel(true, message, 200);
    }

    public static ResponseModel Failure(string message = "Request Failed", int code = 400, IDictionary<string, string[]> errors = null)
    {
        return new ResponseModel(false, message, code, errors);
    }
}

public class ResponseModel<T> : ResponseModel
{
    public T Data { get; set; }

    public static ResponseModel Success(T data, string message = null)
    {
        return new ResponseModel<T>()
        {
            Status = true,
            Message = message ?? "Request was Successful",
            Data = data,
            Code = 200
        };
    }
}

public class ResponseErrorModel : ResponseModel
{
    public static ResponseErrorModel Failure(IDictionary<string, string[]> errors, string message = null)
    {
        return new ResponseErrorModel()
        {
            Status = false,
            Message = message ?? "Request Failed",
            Errors = errors,
            Code = 400
        };
    }

    public new IDictionary<string, string[]> Errors { get; set; }
}
