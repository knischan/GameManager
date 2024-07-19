using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using GameManager.Data_Objects;
using System.Windows.Forms;
using System.Threading.Tasks;
using GameManager.Enums;
using static NtsLib.Utils;
using static GameManager.Base.Utils;
using GameManager.Base;
using System.Collections.Generic;

namespace GameManager {
  public class SocketServer {
    private TcpListener listener;
    private int port;
    public event NetworkEventHandler PayloadReceived;
    public event NetworkEventHandler TransmissionTimedOut;
    public delegate void NetworkEventHandler(object sender, NetworkEventArgs e);

    public SocketServer(int port) {
      this.port = port;
      listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start() {
      listener.Start();
      AcceptClients();
    }

    private async void AcceptClients() {
      while (true) {
        TcpClient client = await listener.AcceptTcpClientAsync();
        HandleClient(client);
      }
    }

    private async void HandleClient(TcpClient client) {
      try {
        using (NetworkStream stream = client.GetStream()) {
          BinaryFormatter formatter = new BinaryFormatter();

          while (client.Connected) {
            if (stream.DataAvailable) {
              Payload payload = (Payload)formatter.Deserialize(stream);
              ProcessPayload(payload);
            } else {
              await Task.Delay(100);
            }
          }
        }
      } catch (Exception ex) {
        Console.WriteLine("Error: " + ex.Message);
      } finally {
        client.Close();
      }
    }

    public async Task TransmitAsync(Payload payload, string ipAddress = null) {
      try {
        if (ipAddress == null) {
          if (payload.RecipientAddress.IsEmpty()) {
            Dialog("Attempted to send payload with no address!");
            return;
          }

          ipAddress = payload.RecipientAddress;
        }

        if (debugMode) LogUI("Transmitting to " + ipAddress.ToString());
        using (var client = new TcpClient()) {
          var connectTask = client.ConnectAsync(ipAddress, port);
          if (await Task.WhenAny(connectTask, Task.Delay(2000)) == connectTask) {
            using (NetworkStream stream = client.GetStream()) {
              BinaryFormatter formatter = new BinaryFormatter();
              formatter.Serialize(stream, payload);
              await stream.FlushAsync();
            }
            if (debugMode) LogUI("Sent " + payload.Command.ToString() + " to " + ipAddress);
          } else {
            var args = new NetworkEventArgs();
            args.Client = cfg.GetClient(payload.RecipientName);
            TransmissionTimedOut?.Invoke(payload, args);
          }
        }
      } catch (Exception ex) {
        Dialog("** Exception during Transmit: " + ex.Message);
      }
    }

    private void ProcessPayload(Payload payload) {
      if (payload == null) return;

      Client client = payload.SenderName.IsEmpty() ? null : cfg.GetClient(payload.SenderName);
      if (debugMode) LogUI("Received a " + payload.Command.ToString() + " payload from client: " + (client == null ? "null" : client.Name));
      var args = new NetworkEventArgs();
      args.Client = client;

      switch (payload.Command) {
      case SystemCommand.Ping:
        Payload pong = CreatePayload(SystemCommand.Pong);
        if (client != null) pong.Games = client.Games;
        pong.Transmit(payload.SenderAddress);
        break;

      case SystemCommand.Pong:
        if (client != null) client.Online = true;
        break;

      case SystemCommand.GameUpdate:
        args.Games = payload.Games;
        break;

      case SystemCommand.GamesRequest:
        Payload pl = new Payload(SystemCommand.GamesList);
        pl.Games = cfg.Games;
        pl.Transmit(payload.SenderAddress);
        args.Games = cfg.Games;
        break;

      case SystemCommand.GamesList:
        args.Games = payload.Games;
        break;

      case SystemCommand.JoinNetwork:
        Client clt = client == null ? new Client() : client;
        clt.Name = payload.SenderName;
        clt.Address = payload.SenderAddress;
        clt.Online = true;
        clt.Games = payload.Games;
        cfg.SetClient(clt.Clone());
        new Payload(SystemCommand.JoinedNetwork).Transmit(payload.SenderAddress);
        SetConfiguration();
        break;

      case SystemCommand.LeaveNetwork:
        if (cfg.Clients.Any(x => x.Name.Matches(client.Name))) cfg.SetClientStatus(client, false);
        break;

      case SystemCommand.LaunchGame:
        if (payload.Game.IsEmpty()) return;

        Game launchGame = cfg.GetGame(payload.Game);
        if (launchGame == null) {
          if (debugMode) LogUI("Launch command received for " + payload.Game + "; that is not a valid game!");
          return;
        }

        if (debugMode) LogUI("Launch command received for " + launchGame.Name);
        GameControl.Launch(launchGame, payload.JoinServer, payload.ServerPassword, payload.ServerPort);
        break;

      case SystemCommand.TerminateGame:
        if (payload.Game.IsEmpty()) return;

        Game terminateGame = cfg.GetGame(payload.Game);
        if (terminateGame == null) {
          LogUI("Termination command received for " + payload.Game + "; that is not a valid game!");
          return;
        }

        LogUI("Termination command received for " + terminateGame.Name);
        GameControl.Terminate(terminateGame);
        break;
      }

      PayloadReceived?.Invoke(payload, args);
    }

    public async void JoinNetwork(string server) {
      try {
        if (server.IsEmpty()) return;

        Payload payload = new Payload(SystemCommand.JoinNetwork);
        payload.Games = cfg.Games;
        await TransmitAsync(payload, server);
      } catch (Exception ex) {
        Dialog("** Exception during JoinNetwork:\n\n" + ex.Message);
      }
    }
  }

  public class NetworkEventArgs : EventArgs {
    public Client Client { get; set; } = null;
    public List<Game> Games { get; set; } = new List<Game>();
  }
}
