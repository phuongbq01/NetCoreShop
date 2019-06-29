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
        public async Task SendMessage(AnnouncementViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
