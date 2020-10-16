using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SahinSms
{
    public class SahinHaberlesme
    {
        private string _token;

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        public void gettoken(string username, string password)
        {
            SahinUser _user = new SahinUser();
            _user.username = username;
            //_user.username = "5065560556";
            _user.password = password;
            //_user.password = "5249184";

            string json = JsonConvert.SerializeObject(_user);
            string result = this.apipost("/core/loginUser", json);
            JObject sonuc = JObject.Parse(result);

            if (sonuc["token"] != null)
            {
                this._token = sonuc["token"].ToString();
            }
        }

        private string apipost(string url, string json)
        {
            string uri = "http://apiv5.basscell.com" + url;
            string result = "";
            using (WebClient wc = new WebClient())
            {
                result = wc.UploadString(uri, json);
            }
            return result;
        }

        public void singlesmsgonder(string baslik, string mesaj, string dil, string flashsms, string gonderimZamani, List<string> numaralar)
        {
            SingleSms smspaket = new SingleSms();
            smspaket.apikey = this._token;
            smspaket.mesajmetni = mesaj;
            smspaket.dil = dil;
            smspaket.flashsms = flashsms;
            smspaket.gonderimzamani = gonderimZamani;
            smspaket.numaralar = numaralar.ToArray();
            smspaket.type = "single";
            smspaket.orjin = baslik;
            string json = JsonConvert.SerializeObject(smspaket);
            string result = this.apipost("/sms/sendsms", json);
        }
    }
}
