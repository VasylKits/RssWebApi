using Microsoft.AspNetCore.Mvc;
using RssManagementWebApi.DTOs;
using RssManagementWebApi.Services.Interfaces;

namespace RssManagementWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeedsController : ControllerBase
{
    public readonly IFeedsService _feedsService;

    /* 
    https://news.un.org/feed/subscribe/en/news/region/europe/feed/rss.xml
    http://rss.cnn.com/rss/edition.rss
    http://rss.cnn.com/rss/edition_entertainment.rss
    http://rss.cnn.com/rss/edition_europe.rss
    http://rss.cnn.com/rss/edition_africa.rss
    */
    public FeedsController(IFeedsService feedsService)
    {
        _feedsService = feedsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFeedsAsync()
    {
        return Ok(await _feedsService.GetAllFeedsAsync());
    }

    [HttpPost]
    public async Task<ActionResult> LoadFeedsAsync([FromBody] LoadModel model)
    {
        return Ok(await _feedsService.LoadFeedsAsync(model));
    }

    [HttpGet("UnreadNews")]
    public async Task<IActionResult> GetFromDateAsync([FromQuery] SearchModel model)
    {
        return Ok(await _feedsService.GetFromDateAsync(model));
    }

    [HttpPut("SetAsRead")]
    public async Task<IActionResult> SetAsReadAsync([FromQuery] string id)
    {
        return Ok(await _feedsService.SetAsReadAsync(id));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFeedsAsync()
    {
        return Ok(await _feedsService.DeleteFeedsAsync());
    }
}