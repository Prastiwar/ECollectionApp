using Microsoft.AspNetCore.Html;

namespace ECollectionApp.WebUI.ViewModels
{
    public class CardViewModel
    {
        public string Header { get; set; }

        public string Message { get; set; }

        public IHtmlContent Footer { get; set; }
    }
}
