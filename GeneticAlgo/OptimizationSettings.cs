using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace GeneticAlgo
{
    public class ParsingParameters
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("multiplier")]
        public decimal Multiplier { get; set; }

        [XmlAttribute("step")]
        public decimal Step { get; set; }
    }

    public class DefaultSet
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("min")]
        public string MinValue { get; set; }

        [XmlAttribute("max")]
        public string MaxValue { get; set; }
    }

    public class Filter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("step")]
        public decimal Step { get; set; }

        [XmlIgnore]
        private long minLimitValue;

        [XmlIgnore]
        private long maxLimitValue;

        [XmlIgnore]
        private string minLimit;

        [XmlIgnore]
        private string maxLimit;

        [XmlElement("minlimit")]
        public string MinLimit { get { return minLimit; }
            set
            {
                long.TryParse(value, out minLimitValue);
                minLimit = value;
            }
        }

        [XmlIgnore]
        public long MinLimitValue { get { return minLimitValue; } }

        [XmlElement("maxlimit")]
        public string MaxLimit
        {
            get { return maxLimit; }
            set
            {
                long.TryParse(value, out maxLimitValue);
                maxLimit = value;
            }
        }

        [XmlIgnore]
        public long MaxLimitValue { get { return maxLimitValue; } }

        [XmlElement("firstpopulation")]
        public bool FirstPopulation { get; set; }

        [XmlElement("columnindex")]
        public string ColumnIndex { get; set; }

        [XmlElement("parsingparameters")]
        public ParsingParameters ParsingParameters { get; set; }

        [XmlElement("defaultset")]
        public DefaultSet DefaultSettings { get; set; }

        public bool HasDefaultSettings { get { return DefaultSettings != null; } }
    }

    [XmlRoot(ElementName = "filters")]
    public class Filters
    {
        [XmlElement("filter")]
        public List<Filter> filters { get; set; }

        public Filter this[int index] { get { return filters[index]; } }

        public int Count { get { return filters.Count; } }

        [XmlIgnore]
        public List<DefaultSet> DefaultSettings { get; private set; }

        [XmlIgnore]
        public Dictionary<string, int> ParametersIndexes { get; private set; }

        public static Filters LoadOptimizationSettings(string filename)
        {
            Filters result = null;
            using (var reader = new StreamReader(filename))
            {
                var deserializer = new XmlSerializer(typeof(Filters));
                result = (Filters)deserializer.Deserialize(reader);
                result.DefaultSettings = new List<DefaultSet>();
                result.ParametersIndexes = new Dictionary<string, int>();
                foreach (var filter in result.filters)
                {
                    if (filter.HasDefaultSettings)
                    {
                        filter.DefaultSettings.Name = filter.ParsingParameters.Name;
                        result.DefaultSettings.Add(filter.DefaultSettings);
                    }
                    int columnIndex;
                    if (!int.TryParse(filter.ColumnIndex, out columnIndex))
                        columnIndex = -1;
                    result.ParametersIndexes[filter.Name] = columnIndex;
                }
            }
            return result;
        }
    }
}
