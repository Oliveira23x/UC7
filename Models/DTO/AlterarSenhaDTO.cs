using System.ComponentModel.DataAnnotations;

namespace apiAutenticacao.Models.DTO
{
    public class AlterarSenhaDTO
    {

        [Required(ErrorMessage = "O email é um campo obrigatório")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido")]
        [StringLength(150, ErrorMessage = "O email deve ter no máximo 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Senha Atual' é obrigatório")]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Nova Senha' é obrigatório")]
        public string NovaSenha { get; set; } = string.Empty;

        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem")]
        [Required(ErrorMessage = "O campo 'Confirmar Nova Senha' é obrigatório")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;




    }

}

   

