using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ChRouterMac
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 3) return;
            try
            {
                await ChangeRouterMacAddressAsync(args[0], args[1], args[2]);
                await CheckInternetAsync();
            }
            catch (Exception e)
            {
                await RestoreRouterMacAddressAsync(args[0], args[1], args[2]);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private static async Task ChangeRouterMacAddressAsync(string gateway,string userName, string password)
        {
            HttpClient httpClient = new HttpClient();
            var authenticationString = $"{userName}:{password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            httpClient.DefaultRequestHeaders.Referrer = new Uri($"http://{gateway}/cgi-bin/timepro.cgi?tmenu=iframe&smenu=hiddenwansetup");
            var randomBytes = GetRandomByte();
            var response = await httpClient.PostAsync($"http://{gateway}/cgi-bin/timepro.cgi?tmenu=main_frame&smenu=main_frame", 
                new StringContent($"tmenu=iframe&smenu=hiddenwansetup&act=save&ocolor=&wan=wan1&ifname=eth2.2&nopassword=0&wan_type=dynamic&allow_private=on&hw_dynamic1={string.Format("{0}", randomBytes[0].ToString("X2"))}&hw_dynamic2={string.Format("{0}", randomBytes[1].ToString("X2"))}&hw_dynamic3={string.Format("{0}", randomBytes[2].ToString("X2"))}&hw_dynamic4={string.Format("{0}", randomBytes[3].ToString("X2"))}&hw_dynamic5={string.Format("{0}", randomBytes[4].ToString("X2"))}&hw_dynamic6={string.Format("{0}", randomBytes[5].ToString("X2"))}&hw_conf_dynamic=on", Encoding.UTF8, "text/plain"));
            Console.WriteLine(response);
        }

        private static async Task CheckInternetAsync()
        {
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10);
            var response = await client.GetAsync("https://api.ip.pe.kr/");
            Console.WriteLine(response.Content);
        }

        private static async Task RestoreRouterMacAddressAsync(string gateway, string userName, string password)
        {
            HttpClient httpClient = new HttpClient();
            var authenticationString = $"{userName}:{password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            httpClient.DefaultRequestHeaders.Referrer = new Uri($"http://{gateway}/cgi-bin/timepro.cgi?tmenu=iframe&smenu=hiddenwansetup");
            var randomBytes = GetRandomByte();
            var response = await httpClient.PostAsync($"http://{gateway}/cgi-bin/timepro.cgi?tmenu=main_frame&smenu=main_frame",
                new StringContent($"tmenu=iframe&smenu=hiddenwansetup&act=save&ocolor=&wan=wan1&ifname=eth2.2&nopassword=0&wan_type=dynamic&allow_private=on", Encoding.UTF8, "text/plain"));
            Console.WriteLine(response);
        }

        private static byte[] GetRandomByte()
        {
            var randomByte = new byte[6];
            new Random().NextBytes(randomByte);
            return randomByte;
        }
    }
}
