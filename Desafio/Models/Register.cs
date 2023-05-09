using System.ComponentModel.DataAnnotations;

namespace Desafio{

    public class Register{
        
        [Key]
        public int IdRegister {get;set;}
        public int idEmployee {get;set;}
        public int IdBusiness {get;set;}
        public DateTime? Date {get;set;}
        public String? RegisterType {get;set;}
    }
}