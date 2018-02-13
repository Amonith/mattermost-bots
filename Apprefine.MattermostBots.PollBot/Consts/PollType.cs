using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.PollBot.Consts
{
    public enum PollType
    {
        /// <summary>
        /// Closed list of available answers.
        /// </summary>
        Closed,
        
        /// <summary>
        /// Everyone can post their own custom answer.
        /// </summary>
        Open
    }
}
