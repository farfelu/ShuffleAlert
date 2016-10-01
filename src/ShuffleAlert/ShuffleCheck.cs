using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShuffleAlert
{
    class ShuffleCheck
    {
        private const string SHUFFLEURL = "https://steamcommunity.com/groups/shuffcat/memberslistxml/?xml=1";

        public event EventHandler ChatDied;

        protected virtual void OnChatDied()
        {
            ChatDied?.Invoke(this, null);
        }

        public async Task Run()
        {
            var memberCount = 0;
            while (true)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var xml = await httpClient.GetStringAsync(SHUFFLEURL);

                        var rex = new Regex(@"<membersInChat>(\d+)<\/membersInChat>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                        var match = rex.Match(xml);
                        var newMemberCount = int.Parse(match.Groups[1].Value);

                        if (memberCount != 0 && newMemberCount == 0)
                        {
                            OnChatDied();
                        }

                        memberCount = newMemberCount;
                    }
                }
                catch { }

                await Task.Delay(10 * 1000);
            }
        }
    }
}
