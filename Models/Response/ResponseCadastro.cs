namespace apiAutenticacao.Models.Reponse
{
    public class ResponseCadastro
    { 
        public bool Erro { get; set; }
        public string Message { get; set; } = string.Empty;
        
        public Usuario? Usuario { get; set; }   
    }
}
