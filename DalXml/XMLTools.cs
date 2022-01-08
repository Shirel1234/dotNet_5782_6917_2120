//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using System.Xml.Serialization;

//namespace Dal
//{
//    static internal class XMLTools
//    {
//        static string dir = @"..\\..\\..\\..\\xml\";
//        public XElement baseStationR;
//        static XMLTools()
//        {
//            if (!Directory.Exists(dir))
//                Directory.CreateDirectory(dir);
//            if (!File.Exists(@"StationsXml.xml"))
//                CreateFiles();
//            else
//                LoadData();
//            DataSourceXml.Initialize();
//            SaveStationsListLinq(DataSourceXml.stations);
//        }

//        #region XElementBaseStation
//        public void CreateFiles()
//        {
//            baseStationRoot = new XElement("baseStations");
//            baseStationRoot.Save(baseStationPath);
//        }
//        public void LoadData()
//        {
//            try
//            {
//                baseStationRoot = XElement.Load(baseStationPath);
//            }
//            catch
//            {
//                throw new Exception("File upload problem");
//            }
//        }
//        public void SaveStationsListLinq(List<BaseStation> stationsList)
//        {
//            baseStationRoot = new XElement("baseStations",
//                                        from bs in stationsList
//                                        select new XElement("baseStation",
//                                        new XElement("id", bs.CodeStation),
//                                        new XElement("name", bs.NameStation),
//                                        new XElement("numOfChargeSlots", bs.ChargeSlots),
//                                        new XElement("location",
//                                            new XElement("longitude", bs.Longitude),
//                                            new XElement("latitude", bs.Latitude)
//                                            )
//                                        )
//                                        );
//            baseStationRoot.Save(baseStationPath);
//        }
//        #endregion


//    #region SaveListToXMLSerializer
//    public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
//        {
//          try
//            {
//                FileStream file = new FileStream(dir + filePath, FileMode.Create);
//                XmlSerializer x = new XmlSerializer(list.GetType());
//                x.Serialize(file, list);
//                file.Close();
//            }
//            catch(Exception ex)
//            {
//                throw new DO.XMLFileLoadCreateException(filePath, $"file to create xml file: {filePath}", ex);
//            }
//        }
//        #endregion

//        #region LoadListFromXMLSerializer
//        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
//        {
//            try
//            {
//                if (File.Exists(dir + filePath))
//                {
//                    List<T> list;
//                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
//                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
//                    list = (List<T>)x.Deserialize(file);
//                    file.Close();
//                    return list;
//                }
//                else
//                    return new List<T>();
//            }
//            catch(Exception ex)
//            {
//                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
//            }
//        }
//        #endregion
//    }
//}   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using DalApi;
using DO;

namespace Dal
{
    static internal class XMLTools
    {
        // static string dir = @"xml\";
        static XMLTools()
        {
            // if (!Directory.Exists(dir))
            //      Directory.CreateDirectory(dir);
        }
        #region SaveLoadWithXElement

        //save a specific xml file according the name- throw exception in case of problems..
        //for the using with XElement..
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new LoadingException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        //load a specific xml file according the name- throw exception in case of problems..
        //for the using with XElement..
        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return XElement.Load(filePath);
                }
                else
                {
                    XElement rootElem = new XElement(filePath);
                    if (filePath == @"configurationXml.xml")
                        rootElem.Add(new XElement("BusLineID", 1));
                    rootElem.Save(filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new LoadingException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer

        //save a complete listin a specific file- throw exception in case of problems..
        //for the using with XMLSerializer..
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new LoadingException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        //load a complete list from a specific file- throw exception in case of problems..
        //for the using with XMLSerializer..
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.LoadingException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

    }
}