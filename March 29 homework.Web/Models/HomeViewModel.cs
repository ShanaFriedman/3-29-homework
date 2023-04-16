using March_29_homework.Data;

namespace March_29_homework.Web.Models
{
    public class HomeViewModel
    {
        public List<Simcha> Simchas { get; set; }
        public int TotalContributors { get; set; }
        public string Message { get; set; }
    }
}
