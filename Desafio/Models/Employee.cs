using System.ComponentModel.DataAnnotations;

namespace Desafio
{
    public class Employee
    {
        [Key]
        public int idEmployee {get;set;}
        public String? Nombre {get;set;}
        public String? Apellido {get;set;}
        public String? Genero {get;set;}
    }
}