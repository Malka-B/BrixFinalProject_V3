using Account.Service.Intefaces;
using Account.Service.Models;
using Account.Share.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Account.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationHistoryController : ControllerBase
    {
        private readonly IOperationHistoryService _operationHistoryService;
        
        public OperationHistoryController(IOperationHistoryService operationHistoryService)
        {
            _operationHistoryService = operationHistoryService;            
        }        

        [HttpGet("GetAccountHistoryFiltered")]
        public IActionResult GetAccountHistoryFiltered([FromQuery] QueryParameters queryParameters)
        {
            PagingReturn pagingReturn = _operationHistoryService
                .GetFilteredInfo(queryParameters);
            var paginationMetadata = new
            {
                totalCount = pagingReturn.Count,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(pagingReturn.Count)
            };
            Response.Headers
                .Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));
            return Ok(pagingReturn.AccountHistory);
        }
    }
}


