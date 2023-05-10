using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio{

    public class Register{
        
        [Key]
        public int IdRegister {get;set;}
        public int IdEmployee {get;set;}

        [ForeignKey(nameof(IdEmployee))]
        public Employee? employee {get;set;}
        public int IdBusiness {get;set;}
        
        [ForeignKey(nameof(IdBusiness))]
        public Business? business {get;set;}
        public DateTime? Date {get;set;}
        public String? RegisterType {get;set;}
    }
}