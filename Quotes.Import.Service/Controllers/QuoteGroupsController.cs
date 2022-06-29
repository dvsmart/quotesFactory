using Microsoft.AspNetCore.Mvc;
using Quotes.Import.Service.ServiceModels;
using Quotes.Import.Service.Services;
using System.Net;

namespace Quotes.Import.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuoteGroupsController : ControllerBase
    {
         
        private readonly ILogger<QuoteGroupsController> _logger;
        private readonly IQuoteGroupsService _quotesGroupMapperService;
        private readonly string groupMappingFilePath;
        public QuoteGroupsController(ILogger<QuoteGroupsController> logger,
            IQuoteGroupsService quotesGroupMapperService,
            IConfiguration configuration)
        {
            _logger = logger;
            _quotesGroupMapperService = quotesGroupMapperService;
            groupMappingFilePath = configuration.GetValue<string>("GroupMappingFilePath");
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductQuoteGroup>), (int)HttpStatusCode.OK)] 
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Get()
        { 
            var response = _quotesGroupMapperService.GetQuoteGroups(groupMappingFilePath);
            if (response != null && response.IsSuccessful)
            {
                return Ok(response.Data);
            }
            else
            { 
                _logger.LogError(response?.ErrorMessage);
                return new ObjectResult("Unexpected Error on getting quote groups details")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            } 
        }
    }
}