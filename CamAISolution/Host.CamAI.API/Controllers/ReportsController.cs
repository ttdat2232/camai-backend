﻿using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Core.Domain.DTO;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Consumers;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/shops")]
[ApiController]
public class ReportsController(IReportService reportService) : ControllerBase
{
    /// <summary>
    /// Only for shop manager
    /// This will get the real-time chart data for their shop
    /// </summary>
    [HttpGet("chart/customer")]
    [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task ShopCustomerAreaChart()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        }
        else
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        // TODO: send data to websocket
    }

    /// <summary>
    /// Only for brand manager
    /// This will get the real-time chart data for one of their shop
    /// </summary>
    [HttpGet("{shopId}/chart/customer")]
    [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task BrandCustomerAreaChart(Guid shopId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var buffer = await reportService.GetClassifierStream(shopId);
            while (true)
            {
                if (buffer.Count > 0)
                {
                    var result = buffer.Read();
                    await SendData(webSocket, result);
                }
                // TODO [Duy]: check connection
            }
            // TODO [Duy]: handle client disconnection
        }
        else
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    }

    private static async Task SendData(WebSocket webSocket, ClassifierModel data)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var json = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));

        while (!receiveResult.CloseStatus.HasValue)
        {
            await webSocket.SendAsync(
                new ArraySegment<byte>(json, 0, receiveResult.Count),
                WebSocketMessageType.Text,
                WebSocketMessageFlags.EndOfMessage,
                CancellationToken.None
            );

            receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None
        );
    }
}
