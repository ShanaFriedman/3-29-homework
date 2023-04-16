using March_29_homework.Data;

namespace March_29_homework.Web.Models
{
    public class ContributoinsViewModel
    {
        public List<Contributor> Contributors { get; set; }
        public Simcha Simcha { get; set; }
        public Dictionary<int, int> ContIdAmount { get; set; }
    }
}
