using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruSayisi;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Csb.YerindeDonusum.Application.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class KdsHub : Hub
{
    private readonly static Dictionary<string, string> _connections = new();
    private readonly IMediator _mediator;

    public KdsHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task OnConnectedAsync()
    {
        // Add the new connection to the list of connections
        await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        // Add the new connection to the list of connections
        _connections.Add(Context.ConnectionId, Context.UserIdentifier);

        await base.OnConnectedAsync();

        var getirBasvuruSayisiQuery = new GetirBasvuruSayisiQuery() { /*Model = model*/ };
        var response = await _mediator.Send(getirBasvuruSayisiQuery);
        await Clients.All.SendAsync("basvuruSayisi", new { basvuruSayisi = response.Result.BasvuruSayisi });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Remove the disconnected connection from the list of connections
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        // Remove the disconnected connection from the list of connections
        _connections.Remove(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        // Call the addMessage method on all clients
        await Clients.All.SendAsync("addMessage", user, message);
    }

    public static IReadOnlyDictionary<string, string> GetConnections()
    {
        return _connections;
    }
}