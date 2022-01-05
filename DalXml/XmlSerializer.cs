using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace XmlSerializer
{
    public enum WeightCategories { easy, medium, heavy };
    public enum Priorities { normal, express, emergency };

    class BaseStation
    {
        public int CodeStation { get; set; }
        public int NameStation { get; set; }
        public int ChargeSlots { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    class Program
    {
        internal static Random r = new Random();
        private static List<BaseStation> getList()
        {
            int id = -1;
            List<BaseStation> stationList;
            stationList = new List<BaseStation>();
            for (int i = 1; i < 3; i++)
            {
                stationList.Add(new BaseStation
                {
                    CodeStation = ++id,
                    NameStation = r.Next(1000, 10000),
                    Longitude = r.NextDouble() + r.Next(30, 34),
                    Latitude = r.NextDouble() + r.Next(34, 37),
                    ChargeSlots = r.Next(5, 6)
                });
            }
            return stationList;
        }
        public static void saveListToXML(List<BaseStation> list, string path)
        {
            XmlSerializer x = new XmlSerializer(list.GetType());
            FileStream fs = new FileStream(path, FileMode.Create);
            x.Serialize(fs, list);
        }
        public static List<BaseStation> loadListFromXML(string path)
        {
            List<BaseStation> list;
            XmlSerializer x = new XmlSerializer(typeof(List<BaseStation>));
            FileStream fs = new FileStream(path, FileMode.Open);
            list = (List<BaseStation>)x.Deserialize(fs);
            return list;
        }
        static void Main(string[] args)
        {
            List<BaseStation> list = getList();

            string path = "xmlBySerilalizer.xml";

            saveListToXML(list, path);

            List<BaseStation> list2 = loadListFromXML(path);

        }
    }


   
}
