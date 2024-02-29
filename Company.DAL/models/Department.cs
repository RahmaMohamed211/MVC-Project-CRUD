using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.models
{
    public class Department 
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime DataOfcreation { get; set; }

        public ICollection<Employee> employees { get; set; } = new HashSet<Employee>();
            //navigation proparty [many]
    }
}
