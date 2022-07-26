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
            await ChangeRouterMacAddressAsync(args[0], args[1], args[2]);
        }

        private static async Task ChangeRouterMacAddressAsync(string gateway,string userName, string password)
        {
            HttpClient httpClient = new HttpClient();
            var authenticationString = $"{userName}:{password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            httpClient.DefaultRequestHeaders.Referrer = new Uri($"http://{gateway}/cgi-bin/timepro.cgi?tmenu=iframe&smenu=hiddenwansetup");
            var randomByte = new byte[1];
            new Random().NextBytes(randomByte);
            var randomByteString = string.Format("{0}", randomByte[0].ToString("X2"));
            var response = await httpClient.PostAsync($"http://{gateway}/cgi-bin/timepro.cgi?tmenu=main_frame&smenu=main_frame", 
                new StringContent($"tmenu=iframe&smenu=hiddenwansetup&act=save&ocolor=&wan=wan1&ifname=eth2.2&nopassword=0&wan_type=dynamic&allow_private=on&fdns_dynamic1=1&fdns_dynamic2=1&fdns_dynamic3=1&fdns_dynamic4=1&sdns_dynamic1=1&sdns_dynamic2=0&sdns_dynamic3=0&sdns_dynamic4=1&dns_dynamic_chk=on&hw_dynamic1=2C&hw_dynamic2=F0&hw_dynamic3=5D&hw_dynamic4=37&hw_dynamic5=28&hw_dynamic6={randomByteString}&hw_conf_dynamic=on", Encoding.UTF8, "text/plain"));
        }
    }
}
