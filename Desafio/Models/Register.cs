using System.ComponentModel.DataAnnotations;

namespace Desafio{

    public class Register{
        
        [Key]
        public int IdRegister {get;set;}
        public int idEmployee {get;set;}
        public int IdBusiness {get;set;}
        public DateTime DateFrom {get;set;}
        public DateTime DateTo {get;set;}
    }
}