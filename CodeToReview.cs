using System;
using System.Collegctions.Generic;
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    // [Code Smell]: Name of clas "people" does not convet the purpose of the class.
    // [Feedback]: Consider renaming to "Individual"/ "Person"/ "User"/ "Customer"
    public class People
    {
        // [Remark]: "Under16" name is misleading it represents like boolean field or offset. 
        // [Code Smell]: It is being used to set a default date of birth for people under 16,
        //  which is not clear, as it will set the date to 15 years ago from now.
        // [Question]: Is this the intended behavior?
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);

        public string Name { get; private set; }

        // [Remark]: Give clear name to property, 'DateOfBirth' as it is more descriptive]
        // [Feedback]: Good use of DateTimeOffset for DOB.
        public DateTimeOffset DOB { get; private set; }

        // [Code Smell]: Not clear why we need to set a default date of birth for people under 16.
        // [Question]: Please clarify the purpose of this constructor.
        public People(string name) : this(name, Under16.Date) { }

        // [Code Smell]: dob parameter is of type DateTime, but it should be DateTimeOffset for consistency.
        // [Feedback]: Consider using DateTimeOffset for DOB to maintain consistency with the rest of the code.]
        public People(string name, DateTime dob)
        {
            Name = name;
            DOB = dob;
        }
    }

    // 
    public class BirthingUnit
    {
        // [Remark]: the comment "MaxItemsToRetrieve" is not relevant
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>
        private List<People> _people;

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param> // [Remark]: The parameter name "j" is not being used in the method. 
        /// <returns>List<object></returns> // [Remark]: The return type is not clear, it should be List<People> instead of List<object>.

        // [Remark]: the input parameter "i" does not convey the purpose of the paramter.
        // Also intention of thes methodis not clear.
        // [Feedback]: Consider renaming to "GeneratePeople" or "CreatePeopleList" for clarity.
        // [Code Smell]: The method is generating random people with hardcoded names.
        public List<People> GetPeople(int i)
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a dandon Name // [Spell Check]: "dandon" should be "random".
                    // [Code Smell]: The Logic for generating names is not clear and seems to be random and hardcoded.
                    // [Feedback]: Consider adding structured logic for generating names.
                    // [Questions]: Is the intention to have a random name generator or should we fetch names from a proper Database or API?]
                    string name = string.Empty;
                    var random = new Random();
                    // [Code Smell]: The random.next(0, 1) will always return 0
                    if (random.Next(0, 1) == 0)
                    {
                        name = "Bob";
                    }
                    else
                    {
                        name = "Betty";
                    }
                    // Adds new people to the list

                    // [Code Smell]: The Logic for generating the date of birth is not clear.]
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0))));
                }
                catch (Exception e)
                {
                    // [Remark]: catching a general Exception is not good practice. Consider catching specific exception.
                    // [Feedback]: Log the exception or handle it appropriately as it is currently throwing new exception which loses original stack trace.
                    // also add appropriate comment.
                    // Dont think this should ever happen 
                    throw new Exception("Something failed in user creation");
                }
            }
            return _people;
        }

/       // [Code Smell]: The method name "GetBobs" is not generic and very specific to "Bob"
        // [Remark]: Consider renaming to "GetPeopleByName" or "GetIndividualsByName".
        // [Feedback]: add a parameter to filter by name, so it can be used for any name also check for "olderThan30" condition whether it is neeeded.
        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
        }

        // [Code Smell]: The method name "GetMarried" does not represent the purpose of the method. 
        // Also the implementation is not clear , as it has input parameter of type People and a string lastName.
        // [Question]: Is the intention to concatenate the name of a person with a last name and return the full name after marriage?

        public string GetMarried(People p, string lastName)
        {
            if (lastName.Contains("test"))
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255)
            {
                (p.Name + " " + lastName).Substring(0, 255);
            }

            return p.Name + " " + lastName;
        }
    }
}