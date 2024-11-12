using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class BaseResponseBO
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "Success";
        public bool IsAuthorized { get; set; } = true;

        /// <summary>
        /// Dynamic object for returning id's etc in case needed
        /// </summary>
        public dynamic? Data { get; set; }
    }

    public class APIResponseBO
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public object? Result { get; set; }
    }

    public abstract class BaseServiceBO
    {
        public APIResponseBO ReturnMethod(bool IsSuccess, string message, object? result = null)
        {
            return new APIResponseBO
            {
                IsSuccess = IsSuccess,
                Message = message,
                Result = result
            };
        }
        public BaseResponseBO ReturnMethod(bool IsSuccess, string message)
        {
            return new BaseResponseBO
            {
                IsSuccess = IsSuccess,
                Message = message
            };
        }
    }

    // Generic response to be used for all APIs with paging.
    public class GenericPagedResponse<T> : GenericResponse<T>
    {
        public GenericPagedResponse()
        {
        }
        public GenericPagedResponse(T data, int totalRecords, int pageSize)
        {
            Data = data;
            TotalRecords = totalRecords;

            var totalPages = ((double)totalRecords / (double)pageSize);
            TotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        }

        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

    // Generic response to be used for all the non-paging APIs 
    public class GenericResponse<T>
    {
        public GenericResponse()
        {
        }

        public GenericResponse(T data)
        {
            Data = data;
        }
        public T? Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "Success";
        public bool IsAuthorized { get; set; } = true;
    }
}
