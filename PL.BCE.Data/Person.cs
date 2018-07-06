using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PL.BCE.Data
{
    public class Person
    {
        //For adding/removing from MVC Views
        public Guid Identifier { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Person Type")]
        public PersonTypes PersonType { get; set; }
        [DisplayName("Benefit Details")]
        public BenefitDetail BenefitDetail { get; set; }

        public Person() : base()
        {
            Identifier = Guid.NewGuid();
        }
    }
}
