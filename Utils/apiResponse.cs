using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace _2020_backend.Utils
{
    public abstract class ResponseDto
    {
        [DataMember]
        public string Status;
    }
    [DataContract]
    public class SuccessResponseDto : ResponseDto
    {
        [DataMember]
        public new string Status { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public object Data { get; set; }
        public SuccessResponseDto()
        {
            Status = "success";
        }
    }
    [DataContract]
    public class ErrorResponseDto : ResponseDto
    {
        [DataMember]
        public new string Status { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public string ErrorMsg { get; set; }
        public ErrorResponseDto()
        {
            Status = "error";
        }
    }
    public  static class ApiResponse
    {
        public static SuccessResponseDto Success(object data) => new SuccessResponseDto { Data = data };
        public static ErrorResponseDto Error(string errorMsg) => new ErrorResponseDto { ErrorMsg = errorMsg };
    }
}
