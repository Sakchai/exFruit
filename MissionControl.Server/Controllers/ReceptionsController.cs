using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MissionControl.Services;
using MissionControl.Shared;
using MissionControl.Shared.Models;
using WebPush;

namespace MissionControl.Server.Controllers
{
    [Route("receptions")]
    [ApiController]
    public class ReceptionsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ReceptionsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reception>>> GetReceptions()
        {
            var receptions = await _db.Receptions
                .OrderByDescending(o => o.CreatedOnUtc)
                .ToListAsync();

            return receptions;
        }

        [Authorize]
        [HttpGet("/receptionItem/{ean}")]
        public async Task<ActionResult<ReceptionItemModel>> GetReceptionItemByEan(string ean)
        {
         
            if (string.IsNullOrWhiteSpace(ean))
                return new ReceptionItemModel();

            var item = await _db.PurchaseItems
                .Where(i => i.ReceptionId == 0 && i.EAN == ean)
                .SingleOrDefaultAsync();

            if (item != null)
                return _mapper.Map<ReceptionItemModel>(item);
            else
                return new ReceptionItemModel();
        }

        [HttpGet("/receptionItems/{id}")]
        public async Task<ActionResult<List<PurchaseItem>>> GetReceptionItems(int id)
        {

            var receptionItems = await _db.PurchaseItems
                .Where(o => o.Reception.Id == id && o.ReceptionId != 0)
                .Include(o => o.Product)
                .ToListAsync();

            return receptionItems;
        }

        [HttpPost("/addReceptionItem/{receptionId}/{ean}")]
        public async Task<ActionResult<int>> AddItem(int receptionId, string ean)
        {
            var item = await _db.PurchaseItems
                .Where(o => o.EAN == ean)
                .SingleOrDefaultAsync();

            item.ReceptionId = receptionId;
            _db.PurchaseItems.Update(item);
            await _db.SaveChangesAsync();

            return item.Id;
        }

        [HttpPost("/updateReceptionItem")]
        public async Task<ActionResult<PurchaseItem>> UpdateReceive(PurchaseItemModel itemModel)
        {
            var item = await _db.PurchaseItems
                .Where(o => o.Id == itemModel.Id)
                .SingleOrDefaultAsync();

            item.ReceivedActualWeight = itemModel.ReceivedActualWeight;
            item.ReceivedCrates = itemModel.ReceivedCrates;
            item.ReceivedCratesWeight = itemModel.ReceivedCratesWeight;
            item.ReceivedDateUtc = DateTime.Now;
            item.ReceivedFruitGradeType = itemModel.ReceivedFruitGradeType;
            item.ReceivedNotes = itemModel.ReceivedNotes;
            item.ReceivedTotalWeight = itemModel.ReceivedTotalWeight;
            item.ReceivingStatusId = itemModel.ReceivingStatusId;

            _db.PurchaseItems.Update(item);
            await _db.SaveChangesAsync();

            return item;
        }
        [HttpDelete("/removeReceptionItemByEan/{ean}")]
        public async Task<ActionResult<int>> RemoveItemByEAN(string ean)
        {
            var item = await _db.PurchaseItems
                .Where(o => o.EAN == ean)
                .SingleOrDefaultAsync();

            item.ReceptionId = 0;
            _db.PurchaseItems.Update(item);
            await _db.SaveChangesAsync();

            return item.Id;
        }

        [HttpDelete("/removeReceptionItemById/{id}")]
        public async Task<ActionResult<int>> RemoveItemById(int id)
        {
            var item = await _db.PurchaseItems
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();

            item.ReceptionId = 0;
            _db.PurchaseItems.Update(item);
            await _db.SaveChangesAsync();

            return id;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> SaveReception(ReceptionModel receptionModel)
        {

            var reception = new Reception
            {
                ReceivedBy = receptionModel.ReceivedBy
            };
            _db.Receptions.Attach(reception);
            await _db.SaveChangesAsync();

            foreach(var item in receptionModel.ReceptionItems)
            {
                var purchaseItem = _db.PurchaseItems.FirstOrDefault(x => x.EAN == item.EAN);
                purchaseItem.ReceptionId = reception.Id;
                purchaseItem.ReceivedActualWeight = item.ReceivedActualWeight;
                purchaseItem.ReceivedCrates = item.ReceivedCrates;
                purchaseItem.ReceivedCratesWeight = item.ReceivedCratesWeight;
                purchaseItem.ReceivedDateUtc = DateTime.Now;
                purchaseItem.ReceivedFruitGradeType = item.ReceivedFruitGradeType;
                purchaseItem.ReceivedNotes = item.ReceivedNotes;
                purchaseItem.ReceivedTotalWeight = item.ReceivedTotalWeight;
                _db.PurchaseItems.Update(purchaseItem);
            };
            await _db.SaveChangesAsync();
            // In the background, send push notifications if possible
            //var subscription = await _db.NotificationSubscriptions.Where(e => e.UserId == GetUserId()).SingleOrDefaultAsync();
            //if (subscription != null)
            //{
            //    _ = TrackAndSendNotificationsAsync(reception, subscription);
            //}

            return reception.Id;
        }



        private static async Task TrackAndSendNotificationsAsync(Reception reception, NotificationSubscription subscription)
        {
            // In a realistic case, some other backend process would track
            // reception delivery progress and send us notifications when it
            // changes. Since we don't have any such process here, fake it.
            //await Task.Delay(PurchaseItem.PreparationDuration);
            //await SendNotificationAsync(reception, subscription, "Your reception has been dispatched!");

            //await Task.Delay(PurchaseItem.DeliveryDuration);
            //await SendNotificationAsync(reception, subscription, "Your reception is now delivered. Enjoy!");
        }

        private static async Task SendNotificationAsync(Reception reception, NotificationSubscription subscription, string message)
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
                    url = $"myreceptions/{reception.Id}",
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
