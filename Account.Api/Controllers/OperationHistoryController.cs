using Account.Service.Intefaces;
using Account.Service.Models;
using Account.Share.Models;
using Account.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationHistoryController : ControllerBase
    {
        private readonly IOperationHistoryService _operationHistoryService;
        private readonly IUrlHelper _urlHelper;

        public OperationHistoryController(IOperationHistoryService operationHistoryService, IUrlHelper urlHelper)
        {
            _operationHistoryService = operationHistoryService;
            _urlHelper = urlHelper;
        }

        //[HttpGet("GetAccountHistory")]
        //public async Task<HistoryDTO> Get([FromQuery] Guid accountId)
        //{
        //    //HistoryDTO accountModel = await _accountService.GetAccountInfoAsync(accountId);
        //    //return _mapper.Map<AccountDTO>(accountModel);
        //    return null;
        //}

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

            var links = CreateLinksForCollection(queryParameters, pagingReturn.Count);

            Response.Headers
                .Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));

            return Ok(new
            {
                value = pagingReturn.AccountHistory,
                links = links
            });
        }

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
             new LinkDto(_urlHelper.Link(nameof(GetAll), new
             {
                 pagecount = queryParameters.PageCount,
                 page = queryParameters.Page,
                 orderby = queryParameters.OrderBy
             }), "self", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAll), new
            {
                pagecount = queryParameters.PageCount,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAll), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAll), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAll), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            return links;
        }

    }
}
