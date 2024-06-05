using System;
using System.Collections.Generic;
using System.Linq;
using Abp;
using Abp.Authorization;
using esign.Authorization;
using esign.DemoUiComponents;
using esign.DemoUiComponents.Dto.Ver1;
using esign.DemoUiComponents.Ver1;
using Microsoft.AspNetCore.Mvc;

namespace esign.DemoUiComponents.Ver1
{
    //[AbpAuthorize(AppPermissions.Pages_DemoUiComponents)]
    public class DemoUiComponentsAppService : esignVersion1AppServiceBase, IDemoUiComponentsAppService
    {
        #region date & time pickers
        [HttpPost]
        public DateFieldOutput SendAndGetDate(DateTime date)
        {
            return new DateFieldOutput
            {
                Date = date
            };
        }

        [HttpPost]
        public DateFieldOutput SendAndGetDateTime(DateTime date)
        {
            return new DateFieldOutput
            {
                Date = date
            };
        }

        [HttpPost]
        public DateRangeFieldOutput SendAndGetDateRange(DateTime startDate, DateTime endDate)
        {
            return new DateRangeFieldOutput
            {
                StartDate = startDate,
                EndDate = endDate
            };
        }

        [HttpPost]
        public DateWithTextFieldOutput SendAndGetDateWithText(SendAndGetDateWithTextInput input)
        {
            return new DateWithTextFieldOutput
            {
                Date = input.Date,
                Text = input.Text
            };
        }

        #endregion

        [HttpGet]
        public List<NameValue<string>> GetCountries(string searchTerm)
        {
            var countries = new List<NameValue<string>>
            {
                new NameValue {Name = "Turkey", Value = "1"},
                new NameValue {Name = "United States of America", Value = "2"},
                new NameValue {Name = "Russian Federation", Value = "3"},
                new NameValue {Name = "France", Value = "4"},
                new NameValue {Name = "Spain", Value = "5"},
                new NameValue {Name = "Germany", Value = "6"},
                new NameValue {Name = "Netherlands", Value = "7"},
                new NameValue {Name = "China", Value = "8"},
                new NameValue {Name = "Italy", Value = "9"},
                new NameValue {Name = "Switzerland", Value = "10"},
                new NameValue {Name = "South Africa", Value = "11"},
                new NameValue {Name = "Belgium", Value = "12"},
                new NameValue {Name = "Brazil", Value = "13"},
                new NameValue {Name = "India", Value = "14"}
            };

            return countries.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        [HttpPost]
        public List<NameValue<string>> SendAndGetSelectedCountries(List<NameValue<string>> selectedCountries)
        {
            return selectedCountries;
        }

        [HttpPost]
        public StringOutput SendAndGetValue(string input)
        {
            return new StringOutput
            {
                Output = input
            };
        }
    }
}
