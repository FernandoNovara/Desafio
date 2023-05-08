using System.ComponentModel.DataAnnotations;

namespace Desafio
{
    public class Business
    {
        [Key]
        public int IdBusiness {get;set;}
        public String? Location {get;set;}
    }
}