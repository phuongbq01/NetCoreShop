using Microsoft.AspNetCore.SignalR;
using OnlineShop.Services.ViewModels.system;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.SignalR
{
    public class ShopHub : Hub
    {
        public static long counter = 0;
        public async Task SendMessage(AnnouncementViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        //public override Task OnConnectedAsync()
        //{
        //    counter += 1;
        //    Clients.All.SendAsync("UpdateCount", counter);
        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    counter -= 1;
        //    Clients.All.SendAsync("UpdateCount", counter);
        //    return base.OnDisconnectedAsync(exception);
        //}
    }
}
