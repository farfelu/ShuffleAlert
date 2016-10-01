using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShuffleAlert
{
    class ShuffleCheck
    {
        private const string SHUFFLEURL = "https://steamcommunity.com/groups/shuffcat/memberslistxml/?xml=1";

        public Action AlertOn { get; set; }
        public Action AlertOff { get; set; }

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

                        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm} MembersInChat: {newMemberCount}");

                        if (memberCount != 0 && newMemberCount == 0)
                        {
                            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm} Triggering alert");
                            AlertOn?.Invoke();
                        }
                        else if (memberCount == 0 && newMemberCount > 0)
                        {
                            AlertOff?.Invoke();
                        }

                        memberCount = newMemberCount;
                    }
                }
                catch { }

                await Task.Delay(60 * 1000);
            }
        }
    }
}
