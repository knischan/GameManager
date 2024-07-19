using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Enums {
  public enum SystemCommand {
    None,
    Ping,
    Pong,
    JoinNetwork,
    JoinedNetwork,
    LeaveNetwork,
    GamesRequest,
    GamesList,
    GameUpdate,
    ShuttingDown,
    LaunchGame,
    TerminateGame
  }
}
