﻿using System.Net.Http;

namespace IfmoSchedule.Tools
{
    public static class VkSender
    {
        //TODO: token
        private const string Access = "place_here";
        private const string Ver = "5.8";
        private const string Chat = "8";
        private const string BaseAddr = "https://api.vk.com/method/messages.send?";

        public static void Send(string message)
        {
            var address = $"{BaseAddr}access_token={Access}&chat_id={Chat}&v={Ver}&message={message}";
            var client = new HttpClient();
            client.GetAsync(address);
        }
    }
}