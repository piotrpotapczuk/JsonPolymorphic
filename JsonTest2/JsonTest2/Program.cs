using JsonTest2.Model;
using System;
using System.IO;
using System.Text.Json;

namespace JsonTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool serialize = false;

            if (serialize)
            {
                MyMessage myMessage = new()
                {
                    Header = new()
                    {
                        HeaderId = 1,
                        Subject = "temat1"
                    },

                    Person = CreateLNaturalPerson()
                };


                string fileName = @"C:\Users\p.potapczuk-adm\source\repos\JsonTest2\JsonTest2\Json\MyJson.json";
                JsonSerializerOptions oprions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IgnoreNullValues = false,

                };
                string jsonString = JsonSerializer.Serialize(myMessage, oprions);
                File.WriteAllText(fileName, jsonString);
            }
            else
            {
                string fileName = @"C:\Users\p.potapczuk-adm\source\repos\JsonTest2\JsonTest2\Json\MyJsonDeserialize.json";
                var myJson = File.ReadAllText(fileName);
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IgnoreNullValues = false
                    

                };
                //options.Converters.Add(new PersonTypeConverter());

                var myMessage = JsonSerializer.Deserialize<MyMessage>(myJson, options);

                Console.WriteLine("stop");




    

            }

            static LegalPerson CreateLegalPerson()
            {
                return new LegalPerson
                {
                    Id = 1,
                    NameLegal = "Legal",
                    CompanyAddress = new CompanyAddress()
                    {
                        CompanyAddressValue = "adres firmowy"
                       
                    }

                };
            }

            static NaturalPerson CreateLNaturalPerson()
            {
                return new NaturalPerson
                {
                    Id = 99,
                    NaturalName = "myNaturalSampleName",
                    NaturalAddress = new NaturalAddress()
                    {
                        NaturalAddressValue = "adres domowy"
                        
                        
                    }



                };
            }


        }
    }
}
