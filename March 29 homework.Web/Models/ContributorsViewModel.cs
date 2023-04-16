using March_29_homework.Data;

namespace March_29_homework.Web.Models
{
    public class ContributorsViewModel
    {
        public List<Contributor> Contributors { get; set; }
        public decimal Total { get; set; }
        public string Message { get; set; }
    }
}
