using Account.Service.Intefaces;
using Account.Service.Models;
using Account.Share.Models;
using Account.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationHistoryController : ControllerBase
    {
        private readonly IOperationHistoryService _operationHistoryService;
        
        public OperationHistoryController(IOperationHistoryService operationHistoryService, IUrlHelper urlHelper)
        {
            _operationHistoryService = operationHistoryService;            
        }
        
        [HttpGet("GetAccountHistory")]        
        public IActionResult GetAll([FromQuery] QueryParameters queryParameters)
        {
            PagingReturn pagingReturn = _operationHistoryService.GetAll(queryParameters);

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
