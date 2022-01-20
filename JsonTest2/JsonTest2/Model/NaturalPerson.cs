namespace JsonTest2.Model
{
    public class NaturalPerson : IPerson
    {

        private string className = "PersonType.NaturalPerson";

        public string ClassName
        {
            get { return className; }

        }

        public int Id { get; set; }
        public string NaturalName { get; set; }
        public NaturalAddress NaturalAddress { get; set; }


    }
}
