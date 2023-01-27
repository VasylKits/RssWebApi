using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssManagementWebApi.DTOs;
using RssManagementWebApi.Services.Interfaces;

// Test links
// https://news.un.org/feed/subscribe/en/news/region/europe/feed/rss.xml
// http://rss.cnn.com/rss/edition_europe.rss

namespace RssManagementWebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class FeedsController : ControllerBase
{
    private readonly IFeedService _feedsService;

    public FeedsController(IFeedService feedsService)
    {
        _feedsService = feedsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFeedsAsync()
    {
        var response = await _feedsService.GetAllFeedsAsync();

        return (response.IsError)
            ? BadRequest(response)
            : Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> LoadFeedsAsync([FromBody] LoadModel model)
    {
        var response = await _feedsService.LoadFeedsAsync(model);

        return (response.IsError)
            ? BadRequest(response)
            : Ok(response);
    }

    [HttpGet("UnreadNews")]
    public async Task<IActionResult> GetFromDateAsync([FromQuery] SearchModel model)
    {
        var response = await _feedsService.GetFromDateAsync(model);

        return (response.IsError)
            ? BadRequest(response)
            : Ok(response);
    }

    [HttpPut("SetAsRead")]
    public async Task<IActionResult> SetAsReadAsync([FromQuery] string id)

    {
        var response = await _feedsService.SetAsReadAsync(id);

        return (response.IsError)
            ? BadRequest(response)
            : Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFeedsAsync()
    {
        var response = await _feedsService.DeleteFeedsAsync();

        return (response.IsError)
            ? BadRequest(response)
            : Ok(response);
    }
}