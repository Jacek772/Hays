using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hays.Application.DTO
{
    public class ErrorDTO
    {
        public ErrorDTO(string errorKey, string errorMessage)
        {
            Errors.Add(errorKey, new List<string> { errorMessage });
        }

        public ErrorDTO(Dictionary<string, List<string>> errors)
        {
            Errors = errors;
        }

        public string Title { get; set; }

        public int Status { get; set; }

        public Dictionary<string, List<string>> Errors { get; }
            = new Dictionary<string, List<string>>();
    }
}
