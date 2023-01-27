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
    public readonly IFeedService _feedsService;

    public FeedsController(IFeedService feedsService)
    {
        _feedsService = feedsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFeedsAsync()
        => Ok(await _feedsService.GetAllFeedsAsync());

    [HttpPost]
    public async Task<ActionResult> LoadFeedsAsync([FromBody] LoadModel model)
        => Ok(await _feedsService.LoadFeedsAsync(model));

    [HttpGet("UnreadNews")]
    public async Task<IActionResult> GetFromDateAsync([FromQuery] SearchModel model)
        => Ok(await _feedsService.GetFromDateAsync(model));

    [HttpPut("SetAsRead")]
    public async Task<IActionResult> SetAsReadAsync([FromQuery] string id)
    => Ok(await _feedsService.SetAsReadAsync(id));

    [HttpDelete]
    public async Task<IActionResult> DeleteFeedsAsync()
    => Ok(await _feedsService.DeleteFeedsAsync());
}