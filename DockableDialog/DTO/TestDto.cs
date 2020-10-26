using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DockableDialog.DTO
{
    [Serializable]
    public class TestDto
    {
        public string test11;
        public string test21;
        [XmlIgnoreAttribute] public string test;
        public TestDto(string _test11, string _test22, string _test)
        {
            test11 = _test11;
            test21 = _test22;
            test = _test;
        }
        public TestDto()
        {

        }

        string test1 { get => test11; set => test11 = value; }
        public string test2 { get => test21; set => test21 = value; }
        public string Test
        {
            get { return test; }
            set { test = value; }
        }

    }
}
