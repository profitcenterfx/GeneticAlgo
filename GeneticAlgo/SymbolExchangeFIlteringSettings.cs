using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace GeneticAlgo
{
    [Serializable]
    [XmlRoot("filter")]
    public class SymbolExchangeFilteringFile
    {
        [XmlElement("symbol")]
        public List<Symbol> symbols { get; set; }
    }

    public class Symbol
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("exchange")]
        public List<Exchange> exchanges { get; set; }
    }

    public class Exchange
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    public class SymbolExchangeFilteringSettings
    {
        private Dictionary<string, Dictionary<string, bool>> filters;

        public bool this[string symbol, string exchange]
        {
            get
            {
                try
                {
                    return filters[symbol][exchange];
                }
                catch
                {
                    return true;
                }
            }
        }

        public SymbolExchangeFilteringSettings(string filename)
        {
            filters = new Dictionary<string, Dictionary<string, bool>>();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof (SymbolExchangeFilteringFile));
                SymbolExchangeFilteringFile xmlConfig = (SymbolExchangeFilteringFile) serializer.Deserialize(new FileStream(filename, FileMode.Open));
                foreach (Symbol s in xmlConfig.symbols)
                {
                    filters[s.Name] = new Dictionary<string, bool>();
                    foreach (Exchange ex in s.exchanges)
                    {
                        filters[s.Name][ex.Name] = false;
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("An error occured when reading filter settings: "+e.Message);
            }
        }
    }
}
