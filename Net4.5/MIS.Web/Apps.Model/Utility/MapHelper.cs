using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Apps.Model.Utility
{
    public class GPoint{
        public double lng { get; set; }
        public double lat { get; set; }

        public string level { get; set; }
    }

    public class MapHelper
    {
        /// <summary>
        /// 地球半径
        /// </summary>
        private const double EarthRadius = 6378.137;

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return d * Math.PI / 180d;
        }


        /// <summary>
        /// 计算两个坐标点之间的距离
        /// </summary>
        /// <param name="firstLatitude">第一个坐标的纬度</param>
        /// <param name="firstLongitude">第一个坐标的经度</param>
        /// <param name="secondLatitude">第二个坐标的纬度</param>
        /// <param name="secondLongitude">第二个坐标的经度</param>
        /// <returns>返回两点之间的距离，单位：公里/千米</returns>                
        public static double GetDistance(double firstLatitude, double firstLongitude, double secondLatitude, double secondLongitude)
        {
            var firstRadLat = Rad(firstLatitude);
            var firstRadLng = Rad(firstLongitude);
            var secondRadLat = Rad(secondLatitude);
            var secondRadLng = Rad(secondLongitude);


            var a = firstRadLat - secondRadLat;
            var b = firstRadLng - secondRadLng;
            var cal = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(firstRadLat)
              * Math.Cos(secondRadLat) * Math.Pow(Math.Sin(b / 2), 2))) * EarthRadius;
            var result = Math.Round(cal * 10000) / 10000;
            return result;
        }


        /// <summary>
        /// 计算两个坐标点之间的距离
        /// </summary>
        /// <param name="firstPoint">第一个坐标点的（纬度,经度）</param>
        /// <param name="secondPoint">第二个坐标点的（纬度,经度）</param>
        /// <returns>返回两点之间的距离，单位：公里/千米</returns>
        public static double GetPointDistance(GPoint firstPoint, GPoint secondPoint)
        {
            return GetDistance(firstPoint.lat, firstPoint.lng, secondPoint.lat, secondPoint.lng);
        }


        // 调用百度地图API根据地址，获取坐标
        public static GPoint GetCoordinate(string address, int timeout=1000)
        {
            if (address == null || address == "")
            {
                return null;
            }

            string AK = "USpxYKZ88GcVXE3WK98zSxVsuvrzDd8o";
            string url = "http://api.map.baidu.com/geocoder/v2/?address=" + address + "&output=json&ak=" + AK;


            string strResult = "";
            try
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.ContentType = "multipart/form-data";
                req.Accept = "*/*";
                //req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
                req.UserAgent = "";
                req.Timeout = timeout;
                req.Method = "GET";
                req.KeepAlive = true;
                HttpWebResponse response = req.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    strResult = sr.ReadToEnd();
                }
                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(strResult);
                if (jsonObj != null)
                {
                    if (jsonObj.ContainsKey("result") && jsonObj["result"] != null)
                    {
                        JObject result = (JObject)jsonObj["result"];
                        JObject location = (JObject)result["location"];


                        GPoint p = new GPoint
                        {
                            lng = (double)location["lng"],
                            lat = (double)location["lat"],
                            level = (string)result["level"],
                        };

                        return p;
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return null;

        }


        public static double GetTowPointDistance(string origin, string destination)
        {
            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
            {
                return 0.0;
            }

            GPoint p1 = GetCoordinate(origin);
            GPoint p2 = GetCoordinate(destination);

            return GetPointDistance(p1, p2);
        }
    }
}
