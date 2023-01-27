using System.Net;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using RssManagementWebApi.DB;
using RssManagementWebApi.DTOs;
using RssManagementWebApi.Models;
using RssManagementWebApi.Services.Interfaces;

namespace RssManagementWebApi.Services.Implementations;

public class FeedService : IFeedService
{
    public readonly ApplicationDbContext _context;

    public FeedService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IBaseResponse<List<FeedResponse>>> GetAllFeedsAsync()
    {
        try
        {
            var responseList = await _context.Feeds.Select(x => new FeedResponse
            {
                Id = x.Id,
                Title = x.Title,
                Url = x.Url,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                Unread = x.Unread
            }).ToListAsync();

            if (responseList.Count == 0)
                return new BaseResponse<List<FeedResponse>>
                {
                    IsError = true,
                    ErrorMessage = "Error! Database is empty! Please, add RSS link!"
                };

            return new BaseResponse<List<FeedResponse>>
            {
                Response = responseList,
                ErrorMessage = $"Request completed! Count = {responseList.Count}"
            };
        }

        catch (Exception ex)
        {
            return new BaseResponse<List<FeedResponse>>
            {
                IsError = true,
                ErrorMessage = $"[GetRssFeedsAsync] : {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<string>> LoadFeedsAsync(LoadModel model)
    {
        try
        {
            WebClient client = new();
            string rss = client.DownloadString(model.RssUrl);

            XDocument xml = XDocument.Parse(rss);
            var feedsList = xml.Descendants("item").Select(x => new Feed
            {
                Title = (string?)x.Element("title"),
                Url = (string?)x.Element("link"),
                Description = (string?)x.Element("description"),
                CreatedDate = (DateTime?)x.Element("pubDate")
            }).ToList();

            await _context.Feeds.AddRangeAsync(feedsList);
            await _context.SaveChangesAsync();

            return new BaseResponse<string>
            {
                Response = $"Success! Feeds added to your database! Count = {_context.Feeds.Count()}"
            };
        }

        catch (Exception ex)
        {
            return new BaseResponse<string>()
            {
                IsError = true,
                ErrorMessage = $"[AddRssFeedAsync] : {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<List<FeedResponse>>> GetFromDateAsync(SearchModel model)
    {
        try
        {
            var responseList = await _context.Feeds.Select(x => new FeedResponse
            {
                Id = x.Id,
                Title = x.Title,
                Url = x.Url,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                Unread = x.Unread
            }).Where(x => (x.CreatedDate != null) && (x.CreatedDate).Value.Year == model.Date.Year
            && (x.CreatedDate).Value.Month == model.Date.Month && (x.CreatedDate).Value.Day == model.Date.Day && x.Unread == true)
            .ToListAsync();

            if (responseList.Count == 0)
                return new BaseResponse<List<FeedResponse>>()
                {
                    IsError = true,
                    ErrorMessage = "There are no unread news for searching date!"
                };

            return new BaseResponse<List<FeedResponse>>()
            {
                Response = responseList,
                ErrorMessage = $"Request completed! Count = {responseList.Count}"
            };
        }

        catch (Exception ex)
        {
            return new BaseResponse<List<FeedResponse>>
            {
                IsError = true,
                ErrorMessage = $"[GetUnreadNewsAsync] : {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<FeedResponse>> SetAsReadAsync(string id)
    {
        try
        {
            var feed = await _context.Feeds.FirstOrDefaultAsync(x => x.Id == id);

            if (feed is null)
                return new BaseResponse<FeedResponse>
                {
                    IsError = true,
                    ErrorMessage = "Error! Not found!"
                };

            feed.Unread = false;

            await _context.SaveChangesAsync();

            return new BaseResponse<FeedResponse>()
            {
                Response = new FeedResponse
                {
                    Id = feed.Id,
                    Title = feed.Title,
                    Url = feed.Url,
                    Description = feed.Description,
                    CreatedDate = feed.CreatedDate,
                    Unread = feed.Unread
                }
            };
        }

        catch (Exception ex)
        {
            return new BaseResponse<FeedResponse>
            {
                IsError = true,
                ErrorMessage = $"[SetFeedsAsReadAsync] : {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<string>> DeleteFeedsAsync()
    {
        try
        {
            var feeds = await _context.Feeds.Select(x => x).ToListAsync();
            _context.Feeds.RemoveRange(feeds);

            await _context.SaveChangesAsync();

            return new BaseResponse<string>
            {
                Response = $"Deleted!"
            };
        }

        catch (Exception ex)
        {
            return new BaseResponse<string>()
            {
                IsError = true,
                ErrorMessage = $"[DeleteFeedsAsync] : {ex.Message}"
            };
        }
    }
}