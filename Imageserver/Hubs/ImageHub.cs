namespace ImageServer.Hubs
{
    public class ImageHub : Hub
    {
        public async Task Send(string info)
        {
            await Clients.All.SendAsync("Notify", info);
        }
    }    
}
