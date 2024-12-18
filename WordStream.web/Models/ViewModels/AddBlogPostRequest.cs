using Microsoft.AspNetCore.Mvc.Rendering;

namespace WordStream.web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool visible { get; set; } 

        // display Tags
        public IEnumerable<SelectListItem> Tags { get; set; }

        //Collect Tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
