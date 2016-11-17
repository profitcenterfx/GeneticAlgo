using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GeneticAlgo
{
    [Serializable]
    public class Settings
    {
        public serversettings agrserver;

        public double maxdrawdown;
        public int start_balance;
        public double trail = 1.5;
        public bool trailopenprice = true;
        public bool use_vwap = false;
        public string agr_protocol = "udp";
        public bool udp_proxy = false;
        public bool use_stops = true;
        public string feed_protocol = "fix";
        public bool book2quote = false;
        public double order_lifetime = 3;
        public int max_positions = 1;
        public double max_server_age = 10;
        public double max_nochange_age = 10;
        public double max_booklevel_age = 10;
        public bool showquotetime = true;
        public int protect_fails = 10;
        public int protect_maxopensliptime = 1;
        public int protect_maxclosesliptime = 1;
        public CommissionsConfig commissions;
        public double commission = 0;
        public int history_points = 10;
        public int history_points_agr = 10;
        public bool sound = true;
        public string provider = "";
        public string provider_trade = "";

        public XmlSlippagesMap slippages;

        public XmlableMap<XmlableMap<feedattributes>> aggregator;
        public XmlableMap<XmlableMap<String>> filter;

        public XmlableMap<symbolattributes> symbols;
    }

    public class serversettings
    {
        public String login;
        public String pass;
        public String host;
        public int port;

        public bool isEmpty
        {
            get
            {
                return login == "" || pass == "" || host == "" || port == 0;
            }
        }
    }

    public class exattributes
    {
        public int max_positions = 1;
        public double treshhold = 0; // points, difference to open position
        public bool reverse = false;
        public int mode = 1; // mode 4 or other
        public int interval = 100; // mode 4 check interval in ms
        public double advance; // points
        public double maxadvance = 1000; // points
        public double minspread = -100000; // points
        public double maxspread; // points
        public double minagrspread = -1000; // points
        public double maxagrspread = 1000000; // points
        public double minagrexspread = -1000; // points
        public double maxagrexspread = 1000000; // points
        public double maxagrsize = 20000; // lots
        public double indent; // points
        public double indentsl; // points
        public double indentclose; // points
        public double tslsl; // points
        public double tslstep; // points
        public double volume; // units
        public string open_by = "ioc"; // mkt, limit ioc, limit gtc
        public string close_by = "ioc"; // mkt, ioc
    }

    [XmlRoot("feedattributes")]
    public class feedattributes
    {
        public int max_positions = 20;
        public double treshhold = -1; // points, difference to open position
        public bool reverse = false;
        public double indent = double.NaN; // points
        public double indentsl = double.NaN; // points
        public double volume = double.NaN; // units
        public double advance = double.NaN; // points
        public double maxadvance = double.NaN; // points
        public double minagrspread = double.NaN; // points
        public double maxagrspread = double.NaN; // points
        public double minagrexspread = double.NaN; // points
        public double maxagrexspread = double.NaN; // points
        public double minagrturn = double.NaN; // pips
        public double maxagrturn = double.NaN; // pips
        public double minfeedturn = double.NaN; // pips
        public double maxfeedturn = double.NaN; // pips
        public double mindiv = double.NaN; // pips
        public double maxdiv = double.NaN; // pips
        public double maxagrstdev = double.NaN;
        public double minagrstdev = double.NaN; // -1 for NaN
        public double maxfeedstdev = double.NaN;
        public double minfeedstdev = double.NaN;
        public double minagrsize = double.NaN; // lots
        public double maxagrsize = double.NaN; // lots
        public double minfeedsize = double.NaN; // lots
        public double maxfeedsize = double.NaN; // lots
        public double minagravespread = double.NaN; // points
        public double maxagravespread = double.NaN; // points
        public double minfeedavespread = double.NaN; // points
        public double maxfeedavespread = double.NaN; // points    
        public double minopentrend = double.NaN;
        public double maxopentrend = double.NaN;
        public double minpowerfeed = double.NaN;
        public double maxpowerfeed = double.NaN;
        public double minpoweragr = double.NaN;
        public double maxpoweragr = double.NaN;
        public double minpowerbbo = double.NaN;
        public double maxpowerbbo = double.NaN;
        public double minpowersum = double.NaN;
        public double maxpowersum = double.NaN;
        public double minaccelfeed = double.NaN;
        public double maxaccelfeed = double.NaN;
        public double minaccelagr = double.NaN;
        public double maxaccelagr = double.NaN;
        public double minaccelbbo = double.NaN;
        public double maxaccelbbo = double.NaN;
        public double minaccelsum = double.NaN;
        public double maxaccelsum = double.NaN;
        public double minmassfeed = double.NaN;
        public double maxmassfeed = double.NaN;
        public double minmassagr = double.NaN;
        public double maxmassagr = double.NaN;
        public double minmassbbo = double.NaN;
        public double maxmassbbo = double.NaN;
        public double minmasssum = double.NaN;
        public double maxmasssum = double.NaN;
        public double minanglefeed = double.NaN;
        public double maxanglefeed = double.NaN;
        public double minangleagr = double.NaN;
        public double maxangleagr = double.NaN;
        public double minanglebbo = double.NaN;
        public double maxanglebbo = double.NaN;
        public double minanglesum = double.NaN;
        public double maxanglesum = double.NaN;
        public double minsigpriceage = double.NaN;
        public double maxsigpriceage = double.NaN;
        public double minfeedpriceage = double.NaN;
        public double maxfeedpriceage = double.NaN;
    }

    [XmlRoot("worktime")]
    public class Worktime : List<String>
    {
    }

    [XmlRoot("exchanges")]
    public class Exchanges : List<String>
    {
    }

    [XmlType(TypeName = "pairs")]
    public class Pairs : List<String>
    {
    }

    [XmlType(TypeName = "syntetic")]
    public class SyntSet : List<Pairs>
    {
    }

    public class symbolattributes
    {
        public Worktime worktime;
        public String convert;
        public int precision = 10000;
        public SyntSet syntetic;
        public XmlableMap<exattributes> exchanges;
    }

    [XmlRoot("exchanges")]
    public class XmlableMap<TValue> : Dictionary<String, TValue>, IXmlSerializable
    {
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty) return;
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                String key = reader.GetAttribute("name");
                reader.ReadStartElement();
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            foreach (String key in this.Keys)
            {
                writer.WriteStartElement("name");
                writer.WriteAttributeString("name", key);
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
            }
        }
    }

    [XmlRoot("slippages")]
    public class XmlSlippagesMap : Dictionary<String, int>, IXmlSerializable
    {
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(int));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty) return;
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                String key = reader.GetAttribute("name");
                int value = reader.ReadElementContentAsInt();
                this.Add(key, value);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(int));
            foreach (String key in this.Keys)
            {
                writer.WriteStartElement("exchange");
                writer.WriteAttributeString("name", key);
                double value = this[key];
                writer.WriteValue(value);
                writer.WriteEndElement();
            }
        }
    }

    [XmlRoot("commission")]
    public class CommissionsConfig : Dictionary<String, double>, IXmlSerializable
    {
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(String));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty) return;
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                String key = reader.GetAttribute("name");
                double value = reader.ReadElementContentAsDouble();
                this.Add(key, value);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer) { }
    }
}
