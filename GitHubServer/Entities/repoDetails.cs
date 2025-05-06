using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entities
{
    public class RepoDetails
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Stars { get; set; }
        public DateTime? LastCommitDate { get; set; }
        public List<string> Languages { get; set; }
        public int PullRequestCount { get; set; }
    }

}
