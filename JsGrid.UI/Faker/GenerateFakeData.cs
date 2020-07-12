using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using Bogus.Extensions.UnitedStates;
using JsDataGrids.DataAccess.Models;

namespace JsDataGrids.UI.Faker
{
    public static class GenerateFakeData
    {

        public static List<Employee> GenerateEmployeeData()
        {
            var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.Id, Guid.NewGuid)
                .RuleFor(e => e.Gender, f => f.Person.Gender.ToString())
                .RuleFor(e => e.FirstName, f => f.Person.FirstName)
                .RuleFor(e => e.LastName, f => f.Person.LastName)
                .RuleFor(e => e.Salutation, (f, e) => f.Name.Prefix2(f.Person.Gender))
                .RuleFor(e => e.MiddleName, f => f.Name.FirstName(f.Person.Gender).OrNull(f, .2f))
               // .RuleFor(e => e.DateOfBirth, (f, e) => f.Date.PastOffset(60, DateTime.Now.AddYears(-18)).Date)
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName.ToLower(), e.LastName.ToLower()))
                .RuleFor(e => e.Phone, f => f.Phone.PhoneNumberFormat())
                .RuleFor(e => e.AddressLine, f => f.Address.StreetAddress())
                .RuleFor(e => e.City, f => f.Address.City())
                .RuleFor(e => e.State, f => f.Address.State())
                .RuleFor(e => e.ZipCode, f => f.Address.ZipCode())
                .RuleFor(e => e.SSN, (f, e) => f.Person.Ssn())
                .RuleFor(e => e.Salary, (f, e) => f.Finance.Amount(4500, 8000, 2).OrDefault(f, 0.2f));
            employeeFaker.Locale = "en_US";
            var employees = employeeFaker.Generate(5);
            
            
            return employees;
        }

    }
}
