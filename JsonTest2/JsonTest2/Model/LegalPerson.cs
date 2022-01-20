using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonTest2.Model
{
    public class LegalPerson : IPerson
    {

        private string className = "PersonType.LegalPerson";

        public string ClassName
        {
            get { return className; }

        }

        public int Id { get; set; }
        public string NameLegal { get; set; }
        public CompanyAddress CompanyAddress { get; set; }


        
    }
}
