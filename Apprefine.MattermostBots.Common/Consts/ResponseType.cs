using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.Common.Consts
{
    public enum ResponseType
    {
        /// <summary>
        /// A normal message to channel
        /// </summary>
        InChannel,

        /// <summary>
        /// Message visible only to the responder
        /// </summary>
        Ephemeral
    }

    public static class ResponseTypesExtensions
    {
        public static string ToMattermostValue(this ResponseType t)
        {
            switch (t)
            {
                case ResponseType.InChannel:
                    return "in_channel";
                case ResponseType.Ephemeral:
                    return "ephemeral";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
