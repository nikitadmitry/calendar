using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Calendar.WebApp.Validation
{
    public class EqualDateValidator : ValidatorBase
    {
        [Parameter]
        public override string Text { get; set; } = "Dates must match";

        [Parameter]
        public DateTime Value { get; set; }

        protected override bool Validate(IRadzenFormComponent component)
        {
            var other = (DateTime) component.GetValue();
            return Value.Date == other.Date;
        }
    }
}