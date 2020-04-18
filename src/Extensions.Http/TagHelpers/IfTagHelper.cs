using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Extensions.TagHelpers
{
    /// <summary>
    /// if "true" the tag will be written in output
    /// </summary>
    [HtmlTargetElement(Attributes = Constants.IncludeIfAttributeName)]
    public class IfAttributeTagHelper : TagHelper
    {
        public override int Order => -1000;

        [HtmlAttributeName(Constants.IncludeIfAttributeName)]
        public bool? Include { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.Attributes.RemoveAll(Constants.IncludeIfAttributeName);

            if (DontRender)
            {
                output.TagName = null;
                output.SuppressOutput();
            }
        }

        private bool DontRender => Include.HasValue && !Include.Value;
    }
}
