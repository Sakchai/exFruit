using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MissionControl.Services;
using MissionControl.Shared;
using MissionControl.Shared.Models;
using WebPush;

namespace MissionControl.Server.Controllers
{
    [Route("sortations")]
    [ApiController]
    //[Authorize]
    public class SortationsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SortationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Sortation>>> GetSortations()
        {
            var sortations = await _db.Sortations
                .OrderByDescending(o => o.CreatedOnUtc)
                .ToListAsync();

            return sortations;
        }

        [HttpGet("/sortationItems/{id}")]
        public async Task<ActionResult<List<PurchaseItem>>> GetSortationItems(int id)
        {
            var sortationItems = await _db.PurchaseItems
                .Where(o => o.Sortation.Id == id && o.SortationId != 0)
                .Include(o => o.Product)
                .ToListAsync();

            return sortationItems;
        }

        [HttpPost("/addSortationItem/{sortationId}/{ean}")]
        public async Task<ActionResult<int>> AddItem(int sortationId, string ean)
        {
            var item = await _db.PurchaseItems
                .Where(o => o.EAN == ean)
                .SingleOrDefaultAsync();

            item.SortationId = sortationId;
            _db.PurchaseItems.Update(item);
            await _db.SaveChangesAsync();

            return item.Id;
        }

        [HttpPost("/updateSortationItem")]
        public async Task<ActionResult<PurchaseItem>> UpdateReceive(SortationItemModel itemModel)
        {
            var item = await _db.PurchaseItems
                .Where(o => o.Id == itemModel.Id)
                .SingleOrDefaultAsync();

            item.SortingCrates = itemModel.SortingCrates;
            item.SortingDateUtc = DateTime.Now;
            item.SortingFruitGradeType = itemModel.SortingFruitGradeType;
            item.SortingNotes = itemModel.SortingNotes;
            item.SortingWastageBad = itemModel.SortingWastageBad;

            _db.PurchaseItems.Update(item);
            await _db.SaveChangesAsync();

            return item;
        }
        [HttpDelete("/removeSortationItemByEan/{ean}")]
        public async Task<ActionResult<int>> RemoveItemByEAN(string ean)
        {
            var item = await _db.PurchaseItems
                .Where(o => o.EAN == ean)
                .SingleOrDefaultAsync();

            item.SortationId = 0;
            _db.PurchaseItems.Update(item);
            await _db.SaveChangesAsync();

            return item.Id;
        }

        [HttpDelete("/removeSortationItemById/{id}")]
        public async Task<ActionResult<int>> RemoveItemById(int id)
        {
            var item = await _db.PurchaseItems
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();

            item.SortationId = 0;
            _db.PurchaseItems.Update(item);
            await _db.SaveChangesAsync();

            return id;
        }

        [HttpPost]
        public async Task<ActionResult<int>> SaveSortation(Sortation sortation)
        {


            // Enforce existence of Pizza.SpecialId and Topping.ToppingId
            // in the database - prevent the submitter from making up
            // new specials and toppings

            _db.Sortations.Attach(sortation);
            await _db.SaveChangesAsync();

            // In the background, send push notifications if possible
            var subscription = await _db.NotificationSubscriptions.Where(e => e.UserId == GetUserId()).SingleOrDefaultAsync();
            if (subscription != null)
            {
                _ = TrackAndSendNotificationsAsync(sortation, subscription);
            }

            return sortation.Id;
        }

        private string GetUserId()
        {
            // This will be the user's twitter username
            return HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        }

        private static async Task TrackAndSendNotificationsAsync(Sortation sortation, NotificationSubscription subscription)
        {
            // In a realistic case, some other backend process would track
            // sortation delivery progress and send us notifications when it
            // changes. Since we don't have any such process here, fake it.
            //await Task.Delay(PurchaseItem.PreparationDuration);
            //await SendNotificationAsync(sortation, subscription, "Your sortation has been dispatched!");

            //await Task.Delay(PurchaseItem.DeliveryDuration);
            //await SendNotificationAsync(sortation, subscription, "Your sortation is now delivered. Enjoy!");
        }

        private static async Task SendNotificationAsync(Sortation sortation, NotificationSubscription subscription, string message)
        {
            // For a real application, generate your own
            var publicKey = "BLC8GOevpcpjQiLkO7JmVClQjycvTCYWm6Cq_a7wJZlstGTVZvwGFFHMYfXt6Njyvgx_GlXJeo5cSiZ1y4JOx1o";
            var privateKey = "OrubzSz3yWACscZXjFQrrtDwCKg-TGFuWhluQ2wLXDo";

            var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
            var vapidDetails = new VapidDetails("mailto:<someone@example.com>", publicKey, privateKey);
            var webPushClient = new WebPushClient();
            try
            {
                var payload = JsonSerializer.Serialize(new
                {
                    message,
                    url = $"mysortations/{sortation.Id}",
                });
                await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error sending push notification: " + ex.Message);
            }
        }
    }
}
