using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;

namespace Extensions.TagHelpers
{
    [HtmlTargetElement(Attributes = Constants.SELECT_IF_ATTRIBUTE_NAME)]
    public class SetSelectedIf : TagHelper
    {
        [HtmlAttributeName(Constants.SELECT_IF_ATTRIBUTE_NAME)]
        public bool? AddAttribute { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.AddAttribute != true)
            {
                return;
            }

            if (output.TagName == "option")
            {
                output.Attributes.Add("selected", "selected");
            }
            else if (output.TagName == "input" && output.Attributes.Any(attr =>
            {
                var attrValue = attr.Value.ToString();
                return attrValue == "checkbox" || attrValue == "radio";
            }))
            {
                output.Attributes.Add("checked", "checked");
            }
            else
            {
                throw new NotSupportedException("Only option/input is supported");
            }
        }
    }
}
