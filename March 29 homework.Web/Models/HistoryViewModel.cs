using March_29_homework.Data;

namespace March_29_homework.Web.Models
{
    public class HistoryViewModel
    {
        public Contributor Contributor { get; set; }
        public List<Actions> Actions { get; set; }
        public string Message { get; set; }

    }
}
