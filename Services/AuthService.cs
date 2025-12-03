using apiAutenticacao.Data;
using apiAutenticacao.Models;
using apiAutenticacao.Models.DTO;
using apiAutenticacao.Models.Response;
using apiAutenticacao.Models.Reponse;   
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;


namespace apiAutenticacao.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseLogin> Login(LoginDTO dadosUsuario)
        {

            Usuario? usuarioEncontrado = await _context.Usuarios.
                FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioEncontrado != null)
            {
                bool isValidPassword = Verify(dadosUsuario.Senha, usuarioEncontrado.Senha);

                if (isValidPassword)
                {
                    return new ResponseLogin
                    {


                        Erro = false,
                        Mensagem = "Login realizado com sucesso",
                        Usuario = usuarioEncontrado



                    };

                }

                return new ResponseLogin
                {
                    Erro = true,
                    Mensagem = "Login não realizado. Email ou Senha incorretos",
                    Usuario = null
                };

            }

            return new ResponseLogin
            {
                Erro = true,
                Mensagem = "Usuário não encontrado",

            };


        }

        public async Task<ResponseCadastro> CadastrarUsuarioAsync(CadastroUsuarioDTO dadosUsuarioCadastro)
        {
            Usuario? usuarioExistente = await _context.Usuarios.
                FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuarioCadastro.Email);

            if (usuarioExistente != null)
            {

                return new ResponseCadastro
                {
                    Erro = true,
                    Message = "Este email já está cadastrado",
                    
                };



            }

            Usuario usuario = new()
            {

                Nome = dadosUsuarioCadastro.Nome,
                Email = dadosUsuarioCadastro.Email,
                Senha = HashPassword(dadosUsuarioCadastro.Senha),


            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new ResponseCadastro
            {
                Erro = false,
                Message = "Usuário cadastrado com sucesso",
                Usuario = usuario
            };




        }

        public async Task<ResponseAlterarSenha> AlterarSenhaAsync(AlterarSenhaDTO dadosAlterarSenha)
        {
            Usuario? usuario = await _context.Usuarios.
                FirstOrDefaultAsync(u => u.Email == dadosAlterarSenha.Email); // serve para buscar o usuario no banco de dados pelo email

            if (usuario == null)
            {

                return new ResponseAlterarSenha
                {
                    Erro = true, // serve para indicar que houve um erro 
                    Message = "Email não encontrado"
                };
            }

            AlterarSenhaDTO dados = new AlterarSenhaDTO(); // serve para criar um novo objeto do tipo AlterarSenhaDTO
            {
                bool isValidPassword = Verify(dadosAlterarSenha.SenhaAtual, usuario.Senha); // serve para verificar se a senha atual está correta
                if (isValidPassword)
                {
                    usuario.Senha = HashPassword(dadosAlterarSenha.NovaSenha); // serve para criptografar a senha

                     // serve para atualizar o usuario no banco de dados
                    await _context.SaveChangesAsync();

                    return new ResponseAlterarSenha // serve para retornar a resposta da alteração de senha
                    {
                        Erro = false,
                        Message = "Senha alterada com sucesso"
                    };

                }
                else
                {

                    return new ResponseAlterarSenha
                    {
                        Erro = true, // serve para indicar que houve um erro
                        Message = "A senha atual está incorreta"
                    };



                }

            }

        }
    }
}

